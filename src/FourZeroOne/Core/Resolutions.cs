#nullable enable
namespace FourZeroOne.Core.Resolutions
{
    using Handles;
    using Resolution;
    using Resolution.Unsafe;
    using SixShaded.FourZeroOne;
    using SixShaded.NotRust;
    using Token;
    using IMemoryAddress = Resolution.IMemoryAddress<Resolution.IResolution>;
    using ResObj = Resolution.IResolution;
    namespace Objects
    {
        public sealed record Number : NoOp
        {
            public required int Value { get; init; }
            public static implicit operator Number(int value) => new() { Value = value };
            public override string ToString() => $"{Value}";
        }
        public sealed record Bool : NoOp
        {
            public required bool IsTrue { get; init; }
            public static implicit operator Bool(bool value) => new() { IsTrue = value };
            public override string ToString() => $"{IsTrue}";
        }
        public sealed record NumRange : NoOp, IMulti<Number>
        {
            public required Number Start { get; init; }
            public required Number End { get; init; }
            public int Count => (Start.Value <= End.Value) ? (End.Value - Start.Value) + 1 : 0;

            public static implicit operator NumRange(Range range)
                => new() { Start = range.Start.Value, End = range.End.Value };
            public IEnumerable<Number> Elements =>
                (Start.Value <= End.Value)
                    ? Start.Sequence(x => x.Value + 1).TakeWhile(x => x.Value <= End.Value)
                    : [];

            public IOption<Number> At(int index)
            {
                return (index <= End.Value - Start.Value)
                    ? new Number() { Value = Start.Value + index }.AsSome()
                    : new None<Number>();
            }

            public override string ToString() => $"{Start}..{End}";
        }

    }
    namespace Instructions
    {
        public sealed record Assign<D> : Instruction where D : class, ResObj
        {
            public required IMemoryAddress<D> Address { get; init; }
            public required D Subject { get; init; }
            public override IMemory TransformMemory(IMemory previousState)
            {
                return previousState.WithObjects([(Address, Subject).Tiple()]);
            }
            public override string ToString() => $"{Address}<-{Subject}";
        }
        

        public sealed record Redact : Instruction
        {
            public required IMemoryAddress Address { get; init; }
            public override IMemory TransformMemory(IMemory context)
            {
                return context.WithClearedAddresses([Address]);
            }
        }
        public sealed record RuleAdd : Instruction
        {
            public required Rule.Unsafe.IRule<ResObj> Rule { get; init; }
            public override IMemory TransformMemory(IMemory state)
            {
                return state.WithRules([Rule]);
            }
            public override string ToString()
            {
                return $"<?>+{Rule}";
            }
        }
    }
    namespace Boxed
    {
        // DEV:
        // consider making MetaFunction construction/data similar to Token and the 'Function' abstractions.
        // where 'ArgAddresses' is to 'ArgTokens' and 'IdentifierX' is to 'ArgX'
        public sealed record MetaFunction<R> : NoOp where R : class, ResObj
        {
            public required DynamicAddress<MetaFunction<R>> SelfIdentifier { get; init; }
            public required IToken<R> Token { get; init; }
            public override string ToString() => $"{SelfIdentifier}(){{{Token}}}";
            public FZOSpec.EStateImplemented.MetaExecute GenerateMetaExecute()
            {
                return new FZOSpec.EStateImplemented.MetaExecute
                {
                    Token = new Tokens.MetaExecuted<R>(Token),
                    ObjectWrites = Iter.Over<(IMemoryAddress<ResObj>, IOption<ResObj>)>
                    ((SelfIdentifier, this.AsSome()))
                    .Tipled()
                };
            }
        }
        public sealed record MetaFunction<RArg1, ROut> : NoOp, IBoxedMetaFunction<ROut>
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            public required DynamicAddress<MetaFunction<RArg1, ROut>> SelfIdentifier { get; init; }
            public required DynamicAddress<RArg1> IdentifierA { get; init; }
            public required IToken<ROut> Token { get; init; }
            IMemoryAddress<IBoxedMetaFunction<ROut>> IBoxedMetaFunction<ROut>.SelfIdentifier => SelfIdentifier;
            IEnumerable<IMemoryAddress<ResObj>> IBoxedMetaFunction<ROut>.ArgAddresses => [IdentifierA];
            public override string ToString() => $"{SelfIdentifier}({IdentifierA})::{{{Token}}}";
            public FZOSpec.EStateImplemented.MetaExecute GenerateMetaExecute(IOption<RArg1> arg1)
            {
                return new FZOSpec.EStateImplemented.MetaExecute
                {
                    Token = new Tokens.MetaExecuted<ROut>(Token),
                    ObjectWrites = Iter.Over<(IMemoryAddress<ResObj>, IOption<ResObj>)>
                    ((SelfIdentifier, this.AsSome()), (IdentifierA, arg1))
                    .Tipled()
                };
            }
        }
        public sealed record MetaFunction<RArg1, RArg2, ROut> : NoOp, IBoxedMetaFunction<ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            public required DynamicAddress<MetaFunction<RArg1, RArg2, ROut>> SelfIdentifier { get; init; }
            public required DynamicAddress<RArg1> IdentifierA { get; init; }
            public required DynamicAddress<RArg2> IdentifierB { get; init; }
            public required IToken<ROut> Token { get; init; }
            IMemoryAddress<IBoxedMetaFunction<ROut>> IBoxedMetaFunction<ROut>.SelfIdentifier => SelfIdentifier;
            IEnumerable<IMemoryAddress<ResObj>> IBoxedMetaFunction<ROut>.ArgAddresses => [IdentifierA, IdentifierB];

            public override string ToString() => $"{SelfIdentifier}({IdentifierA}, {IdentifierB})::{{{Token}}}";
            public FZOSpec.EStateImplemented.MetaExecute GenerateMetaExecute(IOption<RArg1> arg1, IOption<RArg2> arg2)
            {
                return new FZOSpec.EStateImplemented.MetaExecute
                {
                    Token = new Tokens.MetaExecuted<ROut>(Token),
                    ObjectWrites = Iter.Over<(IMemoryAddress<ResObj>, IOption<ResObj>)>
                    ((SelfIdentifier, this.AsSome()), (IdentifierA, arg1), (IdentifierB, arg2))
                    .Tipled()
                };
            }
        }
        public sealed record MetaFunction<RArg1, RArg2, RArg3, ROut> : NoOp, IBoxedMetaFunction<ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            public required DynamicAddress <MetaFunction<RArg1, RArg2, RArg3, ROut>> SelfIdentifier { get; init; }
            public required DynamicAddress<RArg1> IdentifierA { get; init; }
            public required DynamicAddress<RArg2> IdentifierB { get; init; }
            public required DynamicAddress<RArg3> IdentifierC { get; init; }
            public required IToken<ROut> Token { get; init; }

            IMemoryAddress<IBoxedMetaFunction<ROut>> IBoxedMetaFunction<ROut>.SelfIdentifier => SelfIdentifier;

            IEnumerable<IMemoryAddress<ResObj>> IBoxedMetaFunction<ROut>.ArgAddresses => [IdentifierA, IdentifierB, IdentifierC];

            public override string ToString() => $"{SelfIdentifier}({IdentifierA}, {IdentifierB}, {IdentifierC})::{{{Token}}}";
            public FZOSpec.EStateImplemented.MetaExecute GenerateMetaExecute(IOption<RArg1> arg1, IOption<RArg2> arg2, IOption<RArg3> arg3)
            {
                return new FZOSpec.EStateImplemented.MetaExecute
                {
                    Token = new Tokens.MetaExecuted<ROut>(Token),
                    ObjectWrites = Iter.Over<(IMemoryAddress<ResObj>, IOption<ResObj>)>
                    ((SelfIdentifier, this.AsSome()), (IdentifierA, arg1), (IdentifierB, arg2), (IdentifierC, arg3))
                    .Tipled()
                };
            }
        }
        /// <summary>
        /// <b>Strictly for internal workings (e.g. Rule definitions).</b><br></br> 
        /// Not for normal use.
        /// </summary>
        public sealed record OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut> : NoOp, IBoxedMetaFunction<ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where RArg4 : class, ResObj
            where ROut : class, ResObj
        {
            public required DynamicAddress<OverflowingMetaFunction<RArg1, RArg2, RArg3, RArg4, ROut>> SelfIdentifier { get; init; }
            public required DynamicAddress<RArg1> IdentifierA { get; init; }
            public required DynamicAddress<RArg2> IdentifierB { get; init; }
            public required DynamicAddress<RArg3> IdentifierC { get; init; }
            public required DynamicAddress<RArg4> IdentifierD { get; init; }
            public required IToken<ROut> Token { get; init; }

            IMemoryAddress<IBoxedMetaFunction<ROut>> IBoxedMetaFunction<ROut>.SelfIdentifier => SelfIdentifier;

            IEnumerable<IMemoryAddress<ResObj>> IBoxedMetaFunction<ROut>.ArgAddresses => [IdentifierA, IdentifierB, IdentifierC, IdentifierD];

            public override string ToString() => $"{SelfIdentifier}({IdentifierA}, {IdentifierB}, {IdentifierC}, {IdentifierD})::{{{Token}}}";
            public FZOSpec.EStateImplemented.MetaExecute GenerateMetaExecute(IOption<RArg1> arg1, IOption<RArg2> arg2, IOption<RArg3> arg3, IOption<RArg4> arg4)
            {
                return new FZOSpec.EStateImplemented.MetaExecute
                {
                    Token = new Tokens.MetaExecuted<ROut>(Token),
                    ObjectWrites = Iter.Over<(IMemoryAddress<ResObj>, IOption<ResObj>)>
                    ((SelfIdentifier, this.AsSome()), (IdentifierA, arg1), (IdentifierB, arg2), (IdentifierC, arg3), (IdentifierD, arg4))
                    .Tipled()
                };
            }
        }

        public sealed record MetaArgs<R1> : NoOp
            where R1 : class, ResObj
        {
            public required IOption<R1> Arg1 { get; init; }
            public override string ToString() => $"<{Arg1}>";
        }
        public sealed record MetaArgs<R1, R2> : NoOp
            where R1 : class, ResObj
            where R2 : class, ResObj
        {
            public required IOption<R1> Arg1 { get; init; }
            public required IOption<R2> Arg2 { get; init; }
            public override string ToString() => $"<{Arg1},{Arg2}>";
        }
        public sealed record MetaArgs<R1, R2, R3> : NoOp
            where R1 : class, ResObj
            where R2 : class, ResObj
            where R3 : class, ResObj
        {
            public required IOption<R1> Arg1 { get; init; }
            public required IOption<R2> Arg2 { get; init; }
            public required IOption<R3> Arg3 { get; init; }
            public override string ToString() => $"<{Arg1},{Arg2},{Arg3}>";
        }
    }

    public record MergeSpec<H> : ICompositionType where H : ICompositionType
    {

        public static _Private.MergeComponentIdentifier<H, R> MERGE<R>(IComponentIdentifier<H, R> component) where R : class, ResObj => new(component);

    }
    namespace _Private
    {
        // ??
        public interface IMergeIdentifier<in C>
            where C : ICompositionType
        {
            public IComponentIdentifier<C> ForComponentUnsafe { get; }
        }
        public record MergeComponentIdentifier<C, R> : IMergeIdentifier<C>, IComponentIdentifier<MergeSpec<C>, R> where C : ICompositionType where R : class, ResObj
        {
            public IComponentIdentifier<C, R> ForComponent { get; private init; }
            public IComponentIdentifier<C> ForComponentUnsafe => ForComponent;
            public string Package => "CORE";
            public string Identity => $"merge-{ForComponent.Identity}";
            public MergeComponentIdentifier(IComponentIdentifier<C, R> component)
            {
                ForComponent = component;
            }
            public override string ToString() => $"{ForComponent.Identity}*";
        }
    }

    public sealed record Multi<R> : Construct, IMulti<R> where R : class, ResObj
    {
        public IEnumerable<R> Elements => Values.Elements;
        public int Count => Values.Count;
        public override IEnumerable<IInstruction> Instructions => Elements.Map(x => x.Instructions).Flatten();
        public required PSequence<R> Values { get; init; } 
        public Updater<PSequence<R>> dValues { init => Values = value(Values); }
        public IOption<R> At(int index)
        {
            try { return Values.At(index).AsSome(); }
            catch { return new None<R>(); }
        }
        public bool Equals(Multi<R>? other)
        {
            return other is not null && Elements.SequenceEqual(other.Elements);
        }
        public override int GetHashCode()
        {
            return Elements.GetHashCode();
        }
        public override string ToString()
        {
            return $"[{string.Join(", ", Elements.Map(x => x.ToString()))}]";
        }
    }
}