using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Perfection;
using ControlledFlows;
using FourZeroOne;
using MorseCode.ITask;
using PROTO_ZeroxFour_1.Util;

#nullable enable
namespace FourZeroOne.Core.Tokens
{
    using Token;
    using ResObj = Resolution.IResolution;
    using r = Resolutions;
    using ro = Resolutions.Objects;
    using Runtime;
    using Resolution;

    namespace IO
    {
        namespace Select
        {
            public sealed record One<R> : Function<IMulti<R>, R> where R : class, ResObj
            {
                public One(IToken<IMulti<R>> from) : base(from) { }

                protected async override ITask<IOption<R>> Evaluate(IRuntime runtime, IOption<IMulti<R>> fromOpt)
                {
                    return fromOpt.Check(out var from)
                        ? (await runtime.ReadSelection(from.Elements, 1)).RemapAs(x => x.First().NullToNone()).Press()
                        : new None<R>();
                }
                protected override IOption<string> CustomToString() => $"Select({Arg1})".AsSome();
            }

            //make range instead of single int count
            public sealed record Multiple<R> : Function<IMulti<R>, ro.Number, r.Multi<R>> where R : class, ResObj
            {
                public Multiple(IToken<IMulti<R>> from, IToken<ro.Number> count) : base(from, count) { }

                protected override async ITask<IOption<r.Multi<R>>> Evaluate(IRuntime runtime, IOption<IMulti<R>> fromOpt, IOption<ro.Number> countOpt)
                {
                    return (fromOpt.Check(out var from) && countOpt.Check(out var count))
                        ? (await runtime.ReadSelection(from.Elements, count.Value)).RemapAs(v => new r.Multi<R>() { Values = v.ToPSequence() })
                        : new None<r.Multi<R>>();
                }
                protected override IOption<string> CustomToString() => $"SelectMulti({Arg1}, {Arg2})".AsSome();

            }
        }
    }
    namespace Number
    {
        public sealed record Add : PureFunction<ro.Number, ro.Number, ro.Number>
        {
            public Add(IToken<ro.Number> operand1, IToken<ro.Number> operand2) : base(operand1, operand2) { }
            protected override ro.Number EvaluatePure(ro.Number a, ro.Number b) { return new() { Value = a.Value + b.Value }; }
            protected override IOption<string> CustomToString() => $"({Arg1} + {Arg2})".AsSome();
        }
        public sealed record Subtract : PureFunction<ro.Number, ro.Number, ro.Number>
        {
            public Subtract(IToken<ro.Number> operand1, IToken<ro.Number> operand2) : base(operand1, operand2) { }
            protected override ro.Number EvaluatePure(ro.Number a, ro.Number b) { return new() { Value = a.Value - b.Value }; }
            protected override IOption<string> CustomToString() => $"({Arg1} - {Arg2})".AsSome();
        }
        public sealed record Multiply : PureFunction<ro.Number, ro.Number, ro.Number>
        {
            public Multiply(IToken<ro.Number> operand1, IToken<ro.Number> operand2) : base(operand1, operand2) { }
            protected override ro.Number EvaluatePure(ro.Number a, ro.Number b) { return new() { Value = a.Value * b.Value }; }
            protected override IOption<string> CustomToString() => $"({Arg1} * {Arg2})".AsSome();
        }
        namespace Compare
        {
            public sealed record GreaterThan : PureFunction<ro.Number, ro.Number, ro.Bool>
            {
                public GreaterThan(IToken<ro.Number> a, IToken<ro.Number> b) : base(a, b) { }
                protected override ro.Bool EvaluatePure(ro.Number in1, ro.Number in2)
                {
                    return new() { IsTrue = in1.Value > in2.Value };
                }
                protected override IOption<string> CustomToString() => $"({Arg1} > {Arg2})".AsSome();
            }
        }
    }
    namespace Range
    {
        public sealed record Create : PureFunction<ro.Number, ro.Number, ro.NumRange>
        {
            public Create(IToken<ro.Number> min, IToken<ro.Number> max) : base(min, max) { }
            protected override ro.NumRange EvaluatePure(ro.Number in1, ro.Number in2)
            {
                return new() { Start = in1, End = in2 };
            }
        }
        namespace Get
        {
            public sealed record Start : PureFunction<ro.NumRange, ro.Number>
            {
                public Start(IToken<ro.NumRange> range) : base(range) { }
                protected override ro.Number EvaluatePure(ro.NumRange in1)
                {
                    return in1.Start;
                }
            }
            public sealed record End : PureFunction<ro.NumRange, ro.Number>
            {
                public End(IToken<ro.NumRange> range) : base(range) { }
                protected override ro.Number EvaluatePure(ro.NumRange in1)
                {
                    return in1.End;
                }
            }
        }
    }
    namespace Multi
    {
        
        // FIXME: probably should just eval to Nolla if either arg is Nolla
        public sealed record Contains<R> : Function<IMulti<R>, R, ro.Bool> where R : class, ResObj
        {
            public Contains(IToken<IMulti<R>> multi, IToken<R> element) : base(multi, element) { }
            protected override ITask<IOption<ro.Bool>> Evaluate(IRuntime runtime, IOption<IMulti<R>> in1, IOption<R> in2)
            {
                return in2.RemapAs(item => new ro.Bool() { IsTrue = in1.RemapAs(arr => arr.Elements.Contains(item)).Or(false)}).ToCompletedITask();
            }
        }
        public sealed record Union<R> : PureCombiner<IMulti<R>, r.Multi<R>> where R : class, ResObj
        {
            public Union(IEnumerable<IToken<IMulti<R>>> elements) : base(elements) { }
            public Union(params IToken<IMulti<R>>[] elements) : base(elements) { }
            protected override r.Multi<R> EvaluatePure(IEnumerable<IMulti<R>> inputs)
            {
                return new() { Values = inputs.Map(x => x.Elements).Flatten().ToPSequence() };
            }
            protected override IOption<string> CustomToString()
            {
                List<IToken<IMulti<R>>> argList = [.. Args];
                return $"[{string.Join(", ", Args.Map(x => x.ToString()))}]".AsSome();
            }
        }
        public sealed record Intersection<R> : Combiner<IMulti<R>, r.Multi<R>> where R : class, ResObj
        {
            public Intersection(IEnumerable<IToken<IMulti<R>>> sets) : base(sets) { }
            public Intersection(params IToken<IMulti<R>>[] sets) : base(sets) { }
            protected override ITask<IOption<r.Multi<R>>> Evaluate(IRuntime _, IEnumerable<IOption<IMulti<R>>> inputs)
            {
                //OPTIMIZE: does not make full usage of PSequence merging behavior.
                return new r.Multi<R>() { Values = inputs
                    .Map(x => x.RemapAs(y => y.Elements).Or([])).Accumulate((a, b) => a.Intersect(b)).Or([]).ToPSequence() }
                .AsSome().ToCompletedITask();
            }
            protected override IOption<string> CustomToString()
            {
                List<IToken<IMulti<R>>> argList = [.. Args];
                return $"{argList[0]}{argList[1..].AccumulateInto("", (msg, v) => $"{msg}I{v}")}".AsSome();
            }
        }
        public sealed record Exclusion<R> : Function<IMulti<R>, IMulti<R>, r.Multi<R>> where R : class, ResObj
        {
            public Exclusion(IToken<IMulti<R>> from, IToken<IMulti<R>> exclude) : base(from, exclude) { }
            protected override ITask<IOption<r.Multi<R>>> Evaluate(IRuntime _, IOption<IMulti<R>> in1, IOption<IMulti<R>> in2)
            {
                return in1.RemapAs(from => new r.Multi<R>() { Values = in2.RemapAs(sub => from.Elements.Except(sub.Elements)).Or([]).ToPSequence() })
                    .ToCompletedITask();
            }
            protected override IOption<string> CustomToString() => $"{Arg1} - {Arg2}".AsSome();

        }
        public sealed record Yield<R> : PureFunction<R, r.Multi<R>> where R : class, ResObj
        {
            public Yield(IToken<R> value) : base(value) { }
            protected override r.Multi<R> EvaluatePure(R in1)
            {
                return new() { Values = in1.Yield().ToPSequence() };
            }
            protected override IOption<string> CustomToString() => $"^{Arg1}".AsSome();
        }
        public sealed record Count : Function<IMulti<ResObj>, ro.Number>
        {
            public Count(IToken<IMulti<ResObj>> of) : base(of) { }
            protected override ITask<IOption<ro.Number>> Evaluate(IRuntime _, IOption<IMulti<ResObj>> in1)
            {
                return new ro.Number() { Value = in1.RemapAs(x => x.Count).Or(0) }.AsSome().ToCompletedITask();
            }
            protected override IOption<string> CustomToString() => $"{Arg1}.len".AsSome();
        }
        /// <summary>
        /// 1 based because it makes things easier.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        public sealed record GetIndex<R> : Function<IMulti<R>, ro.Number, R> where R : class, ResObj
        {
            public GetIndex(IToken<IMulti<R>> from, IToken<ro.Number> index) : base(from, index) { }
            protected override ITask<IOption<R>> Evaluate(IRuntime _, IOption<IMulti<R>> in1, IOption<ro.Number> in2)
            {
                var o = in1.Check(out var from) && in2.Check(out var index)
                    ? from.At(index.Value - 1)
                    : new None<R>();
                return o.ToCompletedITask();
            }
            protected override IOption<string> CustomToString() => $"{Arg1}[{Arg2}]".AsSome();
        }
    }
    namespace Data
    {
        public sealed record Get<RAddress, RObj> : Function<RAddress, RObj> where RAddress : class, IStateAddress<RObj>, ResObj where RObj : class, ResObj
        {
            public Get(IToken<RAddress> address) : base(address) { }
            protected override ITask<IOption<RObj>> Evaluate(IRuntime runtime, IOption<RAddress> in1)
            {
                return in1.RemapAs(x => runtime.GetState().GetObject(x)).Press().ToCompletedITask();
            }
            protected override IOption<string> CustomToString() => $"*{Arg1}".AsSome();
        }
        public sealed record Insert<RAddress, RObj> : PureFunction<RAddress, RObj, r.Instructions.Assign<RObj>> where RAddress : class, IStateAddress<RObj>, ResObj where RObj : class, ResObj
        {
            public Insert(IToken<RAddress> address, IToken<RObj> obj) : base(address, obj) { }
            protected override r.Instructions.Assign<RObj> EvaluatePure(RAddress in1, RObj in2)
            {
                return new() { Address = in1, Subject = in2 };
            }
            protected override IOption<string> CustomToString() => $"{Arg1} <== {Arg2}".AsSome();
        }
        public sealed record Remove<RAddress> : PureFunction<RAddress, r.Instructions.Redact> where RAddress : class, Resolution.Unsafe.IStateAddress, ResObj
        {
            public Remove(IToken<RAddress> address) : base(address) { }
            protected override r.Instructions.Redact EvaluatePure(RAddress in1)
            {
                return new() { Address = in1 };
            }
            protected override IOption<string> CustomToString() => $"{Arg1} <=X".AsSome();
        }
    }

    // TODO: re-evaluate type restriction usage on this namespace
    // not that it's bad, it just *may* be bad
    namespace Component
    {
        public sealed record Get<C, R> : Token<R> where R : class, ResObj where C : ICompositionType
        {
            public Get(IComponentIdentifier<C, R> identifier, IToken<ICompositionOf<C>> holder) : base(holder)
            {
                _identifier = identifier;
            }
            public override ITask<IOption<R>> Resolve(IRuntime _, IOption<ResObj>[] args)
            {
                return args[0].RemapAs(x => ((ICompositionOf<C>)x).GetComponent(_identifier)).Press().ToCompletedITask();
            }
            protected override IOption<string> CustomToString() => $"{ArgTokens[0]}->{_identifier}".AsSome();
            private readonly IComponentIdentifier<C, R> _identifier;
        }
        public sealed record With<C, R> : Token<ICompositionOf<C>> where R : class, ResObj where C : ICompositionType
        {
            public With(IComponentIdentifier<C, R> identifier, IToken<ICompositionOf<C>> holder, IToken<R> component) : base(holder, component)
            {
                _identifier = identifier;
            }
            public override ITask<IOption<ICompositionOf<C>>> Resolve(IRuntime _, IOption<ResObj>[] args)
            {
                return
                    (args[0].RemapAs(x => (ICompositionOf<C>)x).Check(out var holder)
                    ?  (args[1].RemapAs(x => (R)x).Check(out var component)
                        ? holder.WithComponent(_identifier, component)
                        : holder
                        ).AsSome()
                    : new None<ICompositionOf<C>>()
                    ).ToCompletedITask();
            }
            protected override IOption<string> CustomToString() => $"{ArgTokens[0]}:{{{_identifier}={ArgTokens[1]}}}".AsSome();
            private readonly IComponentIdentifier<C, R> _identifier;
        }
        public sealed record Without<C> : Token<ICompositionOf<C>> where C : ICompositionType
        {
            public Without(Resolution.Unsafe.IComponentIdentifier<C> identifier, IToken<ICompositionOf<C>> holder) : base(holder)
            {
                _identifier = identifier;
            }
            public override ITask<IOption<ICompositionOf<C>>> Resolve(IRuntime _, IOption<ResObj>[] args)
            {
                return args[0].RemapAs(x => ((ICompositionOf<C>)x).WithoutComponents([_identifier])).ToCompletedITask();
            }
            protected override IOption<string> CustomToString() => $"{ArgTokens[0]}:{{{_identifier} X}}".AsSome();
            private readonly Resolution.Unsafe.IComponentIdentifier<C> _identifier;
        }
        public sealed record DoMerge<C> : Function<ICompositionOf<C>, ICompositionOf<r.MergeSpec<C>>, ICompositionOf<C>> where C : ICompositionType
        {
            public DoMerge(IToken<ICompositionOf<C>> in1, IToken<ICompositionOf<r.MergeSpec<C>>> in2) : base(in1, in2) { }

            protected override ITask<IOption<ICompositionOf<C>>> Evaluate(IRuntime runtime, IOption<ICompositionOf<C>> in1, IOption<ICompositionOf<r.MergeSpec<C>>> in2)
            {
                return (in1.Check(out var subject) & in2.Check(out var merger)).ToOptionLazy(() =>
                    (ICompositionOf<C>)subject.WithComponentsUnsafe(
                            merger.ComponentsUnsafe
                            .FilterMap(x => (x.A as r._Private.IMergeIdentifier).NullToNone().RemapAs(y => (y.ForComponentUnsafe, x.B).Tiple()))))
                    .ToCompletedITask();
            }
            protected override IOption<string> CustomToString() => $"{Arg1}>>{Arg2}".AsSome();
        }
    }
    public record Execute<R> : Function<r.Boxed.MetaFunction<R>, R>
        where R : class, ResObj
    {
        public Execute(IToken<r.Boxed.MetaFunction<R>> function) : base(function) { }

        protected override ITask<IOption<R>> Evaluate(IRuntime runtime, IOption<r.Boxed.MetaFunction<R>> in1)
        {
            return in1.Check(out var function)
                ? runtime.MetaExecute(function.Token, [(function.SelfIdentifier, function.AsSome())])
                : new None<R>().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:<>;".AsSome();
    }
    public record Execute<RArg1, ROut> : Function<r.Boxed.MetaFunction<RArg1, ROut>, r.Boxed.MetaArgs<RArg1>, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public Execute(IToken<r.Boxed.MetaFunction<RArg1, ROut>> function, IToken<r.Boxed.MetaArgs<RArg1>> arg) : base(function, arg) { }

        protected override ITask<IOption<ROut>> Evaluate(IRuntime runtime, IOption<r.Boxed.MetaFunction<RArg1, ROut>> in1, IOption<r.Boxed.MetaArgs<RArg1>> in2)
        {
            return in1.Check(out var function) && in2.Check(out var args)
                ? runtime.MetaExecute(function.Token, [(function.SelfIdentifier, function.AsSome()), (function.IdentifierA, args.Arg1)])
                : new None<ROut>().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
    }
    public record Execute<RArg1, RArg2, ROut> : Function<r.Boxed.MetaFunction<RArg1, RArg2, ROut>, r.Boxed.MetaArgs<RArg1, RArg2>, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public Execute(IToken<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> function, IToken<r.Boxed.MetaArgs<RArg1, RArg2>> args) : base(function, args) { }

        protected override ITask<IOption<ROut>> Evaluate(IRuntime runtime, IOption<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> in1, IOption<r.Boxed.MetaArgs<RArg1, RArg2>> in2)
        {
            return in1.Check(out var function) && in2.Check(out var args)
                ? runtime.MetaExecute(function.Token, [(function.SelfIdentifier, function.AsSome()), (function.IdentifierA, args.Arg1), (function.IdentifierB, args.Arg2)])
                : new None<ROut>().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
    }
    public record Execute<RArg1, RArg2, RArg3, ROut> : Function<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>, r.Boxed.MetaArgs<RArg1, RArg2, RArg3>, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public Execute(IToken<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> function, IToken<r.Boxed.MetaArgs<RArg1, RArg2, RArg3>> args) : base(function, args) { }

        protected override ITask<IOption<ROut>> Evaluate(IRuntime runtime, IOption<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> in1, IOption<r.Boxed.MetaArgs<RArg1, RArg2, RArg3>> in2)
        {
            return in1.Check(out var function) && in2.Check(out var args)
                ? runtime.MetaExecute(function.Token, [(function.SelfIdentifier, function.AsSome()), (function.IdentifierA, args.Arg1), (function.IdentifierB, args.Arg2), (function.IdentifierC, args.Arg3)])
                : new None<ROut>().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"!{Arg1}:{Arg2};".AsSome();
    }

    public record ToBoxedArgs<R1> : Function<R1, r.Boxed.MetaArgs<R1>>
        where R1 : class, ResObj
    {
        public ToBoxedArgs(IToken<R1> in1) : base(in1) { }
        protected override ITask<IOption<r.Boxed.MetaArgs<R1>>> Evaluate(IRuntime _, IOption<R1> in1)
        {
            return new r.Boxed.MetaArgs<R1>() { Arg1 = in1 }.AsSome().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"<${Arg1}>".AsSome();
    }
    public record ToBoxedArgs<R1, R2> : Function<R1, R2, r.Boxed.MetaArgs<R1, R2>>
        where R1 : class, ResObj
        where R2 : class, ResObj
    {
        public ToBoxedArgs(IToken<R1> in1, IToken<R2> in2) : base(in1, in2) { }
        protected override ITask<IOption<r.Boxed.MetaArgs<R1, R2>>> Evaluate(IRuntime _, IOption<R1> in1, IOption<R2> in2)
        {
            return new r.Boxed.MetaArgs<R1, R2>() { Arg1 = in1, Arg2 = in2}.AsSome().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"<${Arg1} ${Arg2}>".AsSome();
    }
    public record ToBoxedArgs<R1, R2, R3> : Function<R1, R2, R3, r.Boxed.MetaArgs<R1, R2, R3>>
        where R1 : class, ResObj
        where R2 : class, ResObj
        where R3 : class, ResObj
    {
        public ToBoxedArgs(IToken<R1> in1, IToken<R2> in2, IToken<R3> in3) : base(in1, in2, in3) { }
        protected override ITask<IOption<r.Boxed.MetaArgs<R1, R2, R3>>> Evaluate(IRuntime _, IOption<R1> in1, IOption<R2> in2, IOption<R3> in3)
        {
            return new r.Boxed.MetaArgs<R1, R2, R3>() { Arg1 = in1, Arg2 = in2, Arg3 = in3 }.AsSome().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"<${Arg1} ${Arg2} ${Arg3}>".AsSome();
    }
    
    public record SubEnvironment<ROut> : PureFunction<ResObj, ROut, ROut>
        where ROut : class, ResObj
    {
        public SubEnvironment(IToken<ResObj> envModifiers, IToken<ROut> evalToken) : base(envModifiers, evalToken) { }
        protected override ROut EvaluatePure(ResObj _, ROut in2)
        {
            return in2;
        }
        protected override IOption<string> CustomToString() => $"let {Arg1} in {{{Arg2}}}".AsSome();

    }
    public record IfElse<R> : Function<ro.Bool, r.Boxed.MetaFunction<R>, r.Boxed.MetaFunction<R>, r.Boxed.MetaFunction<R>> where R : class, ResObj
    {
        public IfElse(IToken<ro.Bool> condition, IToken<r.Boxed.MetaFunction<R>> positive, IToken<r.Boxed.MetaFunction<R>> negative) : base(condition, positive, negative) { }
        protected override ITask<IOption<r.Boxed.MetaFunction<R>>> Evaluate(IRuntime runtime, IOption<ro.Bool> in1, IOption<r.Boxed.MetaFunction<R>> in2, IOption<r.Boxed.MetaFunction<R>> in3)
        {
            return in1.RemapAs(x => x.IsTrue ? in2 : in3).Press().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"if {Arg1} then {Arg2} else {Arg3}".AsSome();
    }
 
    public sealed record Fixed<R> : PureValue<R> where R : class, ResObj
    {
        public readonly R Resolution;
        public Fixed(R resolution)
        {
            Resolution = resolution;
        }
        protected override R EvaluatePure()
        {
            return Resolution;
        }
        protected override IOption<string> CustomToString() => $"|{Resolution}|".AsSome();
    }
    public sealed record Nolla<R> : Value<R> where R : class, ResObj
    {
        public Nolla() { }
        protected override ITask<IOption<R>> Evaluate(IRuntime _) { return new None<R>().ToCompletedITask(); }
        protected override IOption<string> CustomToString() => "nolla".AsSome();
    }
    public sealed record Exists : Function<ResObj, ro.Bool>
    {
        public Exists(IToken<ResObj> obj) : base(obj) { }
        protected override ITask<IOption<ro.Bool>> Evaluate(IRuntime _, IOption<ResObj> obj)
        {
            return new ro.Bool() { IsTrue = obj.IsSome() }.AsSome().ToCompletedITask();
        }
    }
    public sealed record DynamicAssign<R> : Token<r.Instructions.Assign<R>> where R : class, ResObj
    {
        public DynamicAssign(DynamicAddress<R> address, IToken<R> obj) : base(obj)
        {
            _assigningAddress = address;
        }
        public override ITask<IOption<r.Instructions.Assign<R>>> Resolve(IRuntime runtime, IOption<ResObj>[] args)
        {
            return args[0].RemapAs(x => new r.Instructions.Assign<R>() { Address = _assigningAddress, Subject = (R)x }).ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"{_assigningAddress}<- {ArgTokens[0]}".AsSome();
        private readonly DynamicAddress<R> _assigningAddress;
    }
    public sealed record DynamicReference<R> : Value<R> where R : class, ResObj
    {
        public DynamicReference(DynamicAddress<R> referenceAddress)
        {
            _referenceAddress = referenceAddress;
        }

        protected override ITask<IOption<R>> Evaluate(IRuntime runtime)
        {
            return runtime.GetState().GetObject(_referenceAddress).ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"&{_referenceAddress}".AsSome();

        private readonly DynamicAddress<R> _referenceAddress;
    }
}