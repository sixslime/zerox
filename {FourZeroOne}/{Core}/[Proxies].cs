using System.Collections;
using System.Collections.Generic;

using Perfection;
using System;

#nullable enable
namespace FourZeroOne.Core.Proxies
{
    using Proxy;
    using Token.Unsafe;
    using Proxy.Unsafe;
    using Token;
    using r = Resolutions;
    using ro = Resolutions.Objects;
    using ResObj = Resolution.IResolution;
    using Resolution;
    public sealed record Direct<TOrig, R> : Proxy<TOrig, R> where TOrig : IToken where R : class, ResObj
    {
        public Direct(IToken<R> token)
        {
            _token = token;
        }
        public Direct<TTo, R> Fix<TTo>() where TTo : IToken<R> => new(_token);
        public override IToken<R> Realize(TOrig _, IOption<Rule.IRule> __) => _token;

        private readonly IToken<R> _token;
    }

    public sealed record ToBoxedFunction<TOrig, R> : Proxy<TOrig, r.Boxed.MetaFunction<R>> where TOrig : IToken where R : class, ResObj
    {
        public ToBoxedFunction(IProxy<TOrig, R> proxy, DynamicAddress<r.Boxed.MetaFunction<R>> idSelf)
        {
            _functionProxy = proxy;
            _vSelf = idSelf;
        }
        public override IToken<r.Boxed.MetaFunction<R>> Realize(TOrig original, IOption<Rule.IRule> rule)
        {
            return new Tokens.Fixed<r.Boxed.MetaFunction<R>>(new() { Token = _functionProxy.Realize(original, rule), SelfIdentifier = _vSelf });
        }
        private readonly IProxy<TOrig, R> _functionProxy;
        private readonly DynamicAddress<r.Boxed.MetaFunction<R>> _vSelf;
    }
    public sealed record ToBoxedFunction<TOrig, RArg1, ROut> : Proxy<TOrig, r.Boxed.MetaFunction<RArg1, ROut>>
        where TOrig : IToken
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public ToBoxedFunction(IProxy<TOrig, ROut> proxy, DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>> idSelf, DynamicAddress<RArg1> id1)
        {
            _functionProxy = proxy;
            _vId1 = id1;
            _vSelf = idSelf;
        }
        public override IToken<r.Boxed.MetaFunction<RArg1, ROut>> Realize(TOrig original, IOption<Rule.IRule> rule)
        {
            return new Tokens.Fixed<r.Boxed.MetaFunction<RArg1, ROut>>(new()
            { Token = _functionProxy.Realize(original, rule), SelfIdentifier = _vSelf, IdentifierA = _vId1 });
        }
        private readonly IProxy<TOrig, ROut> _functionProxy;
        private readonly DynamicAddress<r.Boxed.MetaFunction<RArg1, ROut>> _vSelf;
        private readonly DynamicAddress<RArg1> _vId1;
    }
    public sealed record ToBoxedFunction<TOrig, RArg1, RArg2, ROut> : Proxy<TOrig, r.Boxed.MetaFunction<RArg1, RArg2, ROut>>
        where TOrig : IToken
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public ToBoxedFunction(IProxy<TOrig, ROut> proxy, DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> idSelf, DynamicAddress<RArg1> id1, DynamicAddress<RArg2> id2)
        {
            _functionProxy = proxy;
            _vId1 = id1;
            _vId2 = id2;
            _vSelf = idSelf;
        }
        public override IToken<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> Realize(TOrig original, IOption<Rule.IRule> rule)
        {
            return new Tokens.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, ROut>>(new()
            { Token = _functionProxy.Realize(original, rule), SelfIdentifier = _vSelf, IdentifierA = _vId1, IdentifierB = _vId2 });
        }
        private readonly IProxy<TOrig, ROut> _functionProxy;
        private readonly DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, ROut>> _vSelf;
        private readonly DynamicAddress<RArg1> _vId1;
        private readonly DynamicAddress<RArg2> _vId2;
    }
    public sealed record ToBoxedFunction<TOrig, RArg1, RArg2, RArg3, ROut> : Proxy<TOrig, r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>
        where TOrig : IToken
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public ToBoxedFunction(IProxy<TOrig, ROut> proxy, DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> idSelf, DynamicAddress<RArg1> id1, DynamicAddress<RArg2> id2, DynamicAddress<RArg3> id3)
        {
            _functionProxy = proxy;
            _vSelf = idSelf;
            _vId1 = id1;
            _vId2 = id2;
            _vId3 = id3;
        }
        public override IToken<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> Realize(TOrig original, IOption<Rule.IRule> rule)
        {
            return new Tokens.Fixed<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>>(new()
            { Token = _functionProxy.Realize(original, rule), SelfIdentifier = _vSelf, IdentifierA = _vId1, IdentifierB = _vId2, IdentifierC = _vId3 });
        }
        private readonly IProxy<TOrig, ROut> _functionProxy;
        private readonly DynamicAddress<r.Boxed.MetaFunction<RArg1, RArg2, RArg3, ROut>> _vSelf;
        private readonly DynamicAddress<RArg1> _vId1;
        private readonly DynamicAddress<RArg2> _vId2;
        private readonly DynamicAddress<RArg3> _vId3;
    }

    public sealed record CombinerTransform<TNew, TOrig, RArg, ROut> : Proxy<TOrig, ROut>
        where TOrig : IHasCombinerArgs<RArg>, IToken<ROut>
        where TNew : Token.ICombiner<RArg, ROut>
        where RArg : class, ResObj
        where ROut : class, ResObj
    {
        public override IToken<ROut> Realize(TOrig original, IOption<Rule.IRule> rule)
        {
            return (TNew)typeof(TNew).GetConstructor(new Type[] { typeof(IEnumerable<IToken<RArg>>) })
                .Invoke(new object[] { original.Args.Map(x => RuleApplied(rule, x)) }) ;
        }
    }

    public record Combiner<TNew, TOrig, RArgs, ROut> : FunctionProxy<TOrig, ROut>
        where TNew : Token.ICombiner<RArgs, ROut>
        where TOrig : IToken
        where RArgs : class, ResObj
        where ROut : class, ResObj
    {
        public Combiner(IEnumerable<IProxy<TOrig, RArgs>> proxies) : base(proxies) { }

        protected override IToken<ROut> ConstructFromArgs(TOrig _, List<IToken> tokens)
        {
            return (IToken<ROut>)typeof(TNew).GetConstructor(new Type[] { typeof(IEnumerable<IToken<RArgs>>) })
                .Invoke(new object[] { tokens.Map(x => (IToken<RArgs>)x) });
        }
    }

    public sealed record SubEnvironment<TOrig, ROut> : Proxy<TOrig, ROut>
        where TOrig : IToken
        where ROut : class, ResObj
    {
        public IProxy<TOrig, ROut> SubTokenProxy { get; init; }
        public SubEnvironment(IProxy<TOrig, IMulti<ResObj>> environment)
        {
            _envModifiers = environment;
        }
        public override IToken<ROut> Realize(TOrig original, IOption<Rule.IRule> rule)
        {
            return new Tokens.SubEnvironment<ROut>(_envModifiers.Realize(original, rule), SubTokenProxy.Realize(original, rule));
        }
        private readonly IProxy<TOrig, IMulti<ResObj>> _envModifiers;
    }
    public sealed record Variable<TOrig, R> : Proxy<TOrig, r.Instructions.VariableAssign<R>>
        where TOrig : IToken
        where R : class, ResObj
    {
        public Variable(DynamicAddress<R> identifier, IProxy<TOrig, R> proxy)
        {
            _identifier = identifier;
            _objectProxy = proxy;
        }
        public override IToken<r.Instructions.VariableAssign<R>> Realize(TOrig original, IOption<Rule.IRule> rule)
        {
            return new Tokens.Variable<R>(_identifier, _objectProxy.Realize(original, rule));
        }

        private readonly DynamicAddress<R> _identifier;
        private readonly IProxy<TOrig, R> _objectProxy;
    }

    public sealed record IfElse<TOrig, R> : Proxy<TOrig, r.Boxed.MetaFunction<R>> where TOrig : IToken where R : class, ResObj
    {
        public readonly IProxy<TOrig, ro.Bool> Condition;
        public IProxy<TOrig, r.Boxed.MetaFunction<R>> PassProxy { get; init; }
        public IProxy<TOrig, r.Boxed.MetaFunction<R>> FailProxy { get; init; }
        public IfElse(IProxy<TOrig, ro.Bool> condition)
        {
            Condition = condition;
        }
        public override IToken<r.Boxed.MetaFunction<R>> Realize(TOrig original, IOption<Rule.IRule> rule)
        {
            return new Tokens.IfElse<R>(Condition.Realize(original, rule), PassProxy.Realize(original, rule), FailProxy.Realize(original, rule));
        }
    }
    // ---- [ OriginalArgs ] ----
    public sealed record OriginalArg1<TOrig, RArg> : Proxy<TOrig, RArg> where TOrig : IHasArg1<RArg> where RArg : class, ResObj
    {
        public override IToken<RArg> Realize(TOrig original, IOption<Rule.IRule> rule)
        {
            return RuleApplied(rule, original.Arg1);
        }
    }
    public sealed record OriginalArg2<TOrig, RArg> : Proxy<TOrig, RArg> where TOrig : IHasArg2<RArg> where RArg : class, ResObj
    {
        public override IToken<RArg> Realize(TOrig original, IOption<Rule.IRule> rule)
        {
            return RuleApplied(rule, original.Arg2);
        }
    }
    public sealed record OriginalArg3<TOrig, RArg> : Proxy<TOrig, RArg> where TOrig : IHasArg3<RArg> where RArg : class, ResObj
    {
        public override IToken<RArg> Realize(TOrig original, IOption<Rule.IRule> rule)
        {
            return RuleApplied(rule, original.Arg3);
        }
    }
    // --------

    // ---- [ Functions ] ----
    public record Function<TNew, TOrig, RArg1, ROut> : FunctionProxy<TOrig, ROut>
        where TOrig : IToken
        where TNew : IFunction<RArg1, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public Function(IProxy<TOrig, RArg1> in1) : base(in1) { }
        protected override IToken<ROut> ConstructFromArgs(TOrig _, List<IToken> tokens)
        {
            return (IToken<ROut>)
                typeof(TNew).GetConstructor(new Type[] { typeof(IToken<RArg1>) })
                .Invoke(tokens.ToArray());
        }
    }
    public record Function<TNew, TOrig, RArg1, RArg2, ROut> : FunctionProxy<TOrig, ROut>
        where TOrig : IToken
        where TNew : IFunction<RArg1, RArg2, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public Function(IProxy<TOrig, RArg1> in1, IProxy<TOrig, RArg2> in2) : base(in1, in2) { }
        protected override IToken<ROut> ConstructFromArgs(TOrig _, List<IToken> tokens)
        {
            return (IToken<ROut>)
                typeof(TNew).GetConstructor(new Type[] { typeof(IToken<RArg1>), typeof(IToken<RArg2>) })
                .Invoke(tokens.ToArray());
        }
    }
    public record Function<TNew, TOrig, RArg1, RArg2, RArg3, ROut> : FunctionProxy<TOrig, ROut>
        where TOrig : IToken
        where TNew : IFunction<RArg1, RArg2, RArg3, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public Function(IProxy<TOrig, RArg1> in1, IProxy<TOrig, RArg2> in2, IProxy<TOrig, RArg3> in3) : base(in1, in2, in3) { }
        protected override IToken<ROut> ConstructFromArgs(TOrig _, List<IToken> tokens)
        {
            return (IToken<ROut>)
                typeof(TNew).GetConstructor(new Type[] { typeof(IToken<RArg1>), typeof(IToken<RArg2>), typeof(IToken<RArg3>) })
                .Invoke(tokens.ToArray());
        }
    }
    // --------

    [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
    public record RecursiveStart<TOrig, RArg1, RArg2, RArg3, ROut> : FunctionProxy<TOrig, ROut>
        where TOrig : IToken
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public required IProxy<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>, ROut> RecursiveProxy { get; init; }
        public RecursiveStart(IProxy<TOrig, RArg1> in1, IProxy<TOrig, RArg2> in2, IProxy<TOrig, RArg3> in3) : base(in1, in2, in3) { }
        protected override IToken<ROut> ConstructFromArgs(TOrig _, List<IToken> tokens)
        {
            return new Tokens.Recursive<RArg1, RArg2, RArg3, ROut>(
                (IToken<RArg1>)tokens[0],
                (IToken<RArg2>)tokens[1],
                (IToken<RArg3>)tokens[2])
            { RecursiveProxy = RecursiveProxy };
        }
    }
    [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
    public record RecursiveStart<TOrig, RArg1, RArg2, ROut> : FunctionProxy<TOrig, ROut>
        where TOrig : IToken
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public required IProxy<Tokens.Recursive<RArg1, RArg2, ROut>, ROut> RecursiveProxy { get; init; }
        public RecursiveStart(IProxy<TOrig, RArg1> in1, IProxy<TOrig, RArg2> in2) : base(in1, in2) { }
        protected override IToken<ROut> ConstructFromArgs(TOrig _, List<IToken> tokens)
        {
            return new Tokens.Recursive<RArg1, RArg2, ROut>(
                (IToken<RArg1>)tokens[0],
                (IToken<RArg2>)tokens[1])
            { RecursiveProxy = RecursiveProxy };
        }
    }
    [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
    public record RecursiveStart<TOrig, RArg1, ROut> : FunctionProxy<TOrig, ROut>
        where TOrig : IToken
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public required IProxy<Tokens.Recursive<RArg1, ROut>, ROut> RecursiveProxy { get; init; }
        public RecursiveStart(IProxy<TOrig, RArg1> in1) : base(in1) { }
        protected override IToken<ROut> ConstructFromArgs(TOrig _, List<IToken> tokens)
        {
            return new Tokens.Recursive<RArg1, ROut>(
                (IToken<RArg1>)tokens[0])
            { RecursiveProxy = RecursiveProxy };
        }
    }

    // ---- [ Recursive Call] ----
    [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
    public record RecursiveCall<RArg1, ROut> : FunctionProxy<Tokens.Recursive<RArg1, ROut>, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public RecursiveCall(IProxy<Tokens.Recursive<RArg1, ROut>, RArg1> in1) : base(in1) { }
        protected override IToken<ROut> ConstructFromArgs(Tokens.Recursive<RArg1, ROut> original, List<IToken> tokens)
        {
            return new Tokens.Recursive<RArg1, ROut>((IToken<RArg1>)tokens[0]) { RecursiveProxy = original.RecursiveProxy };
        }
    }
    [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
    public record RecursiveCall<RArg1, RArg2, ROut> : FunctionProxy<Tokens.Recursive<RArg1, RArg2, ROut>, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public RecursiveCall(IProxy<Tokens.Recursive<RArg1, RArg2, ROut>, RArg1> in1, IProxy<Tokens.Recursive<RArg1, RArg2, ROut>, RArg2> in2) : base(in1, in2) { }
        protected override IToken<ROut> ConstructFromArgs(Tokens.Recursive<RArg1, RArg2, ROut> original, List<IToken> tokens)
        {
            return new Tokens.Recursive<RArg1, RArg2, ROut>(
                (IToken<RArg1>)tokens[0],
                (IToken<RArg2>)tokens[1])
            { RecursiveProxy = original.RecursiveProxy };
        }
    }
    [Obsolete("Will be removed. Use self referencing MetaFunctions.", true)]
    public record RecursiveCall<RArg1, RArg2, RArg3, ROut> : FunctionProxy<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public RecursiveCall(IProxy<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>, RArg1> in1, IProxy<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>, RArg2> in2, IProxy<Tokens.Recursive<RArg1, RArg2, RArg3, ROut>, RArg3> in3) : base(in1, in2, in3) { }
        protected override IToken<ROut> ConstructFromArgs(Tokens.Recursive<RArg1, RArg2, RArg3, ROut> original, List<IToken> tokens)
        {
            return new Tokens.Recursive<RArg1, RArg2, RArg3, ROut>(
                (IToken<RArg1>)tokens[0],
                (IToken<RArg2>)tokens[1],
                (IToken<RArg3>)tokens[2])
            { RecursiveProxy = original.RecursiveProxy };
        }
    }

    // --------
}