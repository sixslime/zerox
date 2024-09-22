
using System.Collections.Generic;
using System.Threading.Tasks;
using Perfection;
using ControlledFlows;
#nullable enable
namespace FourZeroOne.Token
{
    using ResObj = Resolution.IResolution;
    using Runtime;
    using r = Core.Resolutions;

    public interface IToken<out R> : Unsafe.IToken where R : class, ResObj
    {
        // "ToNodes(IRuntime runtime)".
        // "Resolve(IOption<ResObj>[]...)"
        public ICeasableFlow<IOption<R>> Resolve(IRuntime runtime, IOption<ResObj>[] args);
    }
    
    public sealed record VariableIdentifier<R> : Unsafe.VariableIdentifier where R : class, ResObj
    {
        public VariableIdentifier() : base() { }
        public override string ToString()
        {
            return $"[&{typeof(R).Name}:{_value}]";
        }
    }
    public abstract record Token<R> : IToken<R> where R : class, ResObj
    {
        public Unsafe.IToken[] ArgTokens => _argTokens;
        public Token(params Unsafe.IToken[] args)
        {
            _argTokens = args;
        }
        public Token(IEnumerable<Unsafe.IToken> args) : this(args.AsList().ToArray()) { }
        public abstract ICeasableFlow<IOption<R>> Resolve(IRuntime runtime, IOption<ResObj>[] args);
        public ICeasableFlow<IOption<ResObj>> ResolveUnsafe(IRuntime runtime, IOption<ResObj>[] args) { return Resolve(runtime, args); }
        protected virtual IOption<string> CustomToString() => new None<string>();

        private Unsafe.IToken[] _argTokens;

        public sealed override string ToString() => CustomToString().Check(out var custom)
                ? custom
                : $"{this.GetType().Name}( {_argTokens.AccumulateInto("", (msg, arg) => $"{msg}{arg} ")})";
    }

    #region Functions
        // ---- [ Functions ] ----

    public interface IFunction<RArg1, ROut> : Unsafe.IHasArg1<RArg1>, Unsafe.IFunction<ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    { }
    public interface IFunction<RArg1, RArg2, ROut> : Unsafe.IHasArg1<RArg1>, Unsafe.IHasArg2<RArg2>, Unsafe.IFunction<ROut>
    where RArg1 : class, ResObj
    where RArg2 : class, ResObj
    where ROut : class, ResObj
    { }
    public interface IFunction<RArg1, RArg2, RArg3, ROut> : Unsafe.IHasArg1<RArg1>, Unsafe.IHasArg2<RArg2>, Unsafe.IHasArg3<RArg3>, Unsafe.IFunction<ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    { }
    public interface ICombiner<RArgs, ROut> : Unsafe.IHasCombinerArgs<RArgs>, Unsafe.IFunction<ROut>
    where RArgs : class, ResObj
    where ROut : class, ResObj
    { }

    public abstract record Value<R> : Token<R> where R : class, ResObj
    {
        public sealed override ICeasableFlow<IOption<R>> Resolve(IRuntime runtime, IOption<ResObj>[] _)
        {
            return Evaluate(runtime);
        }
        protected Value() : base() { }
        protected abstract ICeasableFlow<IOption<R>> Evaluate(IRuntime runtime);
    }
    public abstract record PureValue<R> : Value<R> where R : class, ResObj
    {
        protected PureValue() : base() { }
        protected sealed override ICeasableFlow<IOption<R>> Evaluate(IRuntime _)
        {
            return ControlledFlow.Resolved(EvaluatePure().AsSome());
        }
        protected abstract R EvaluatePure();
    }
    /// <summary>
    /// Core.Tokens that inherit must have a constructor matching: <br></br>
    /// <code>(IToken&lt;<typeparamref name="RArg1"/>&gt;)</code>
    /// </summary>
    /// <typeparam name="RArg1"></typeparam>
    public abstract record Function<RArg1, ROut> : Token<ROut>,
        IFunction<RArg1, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        public sealed override ICeasableFlow<IOption<ROut>> Resolve(IRuntime runtime, IOption<ResObj>[] args)
        {
            return Evaluate(runtime, args[0].RemapAs(x => (RArg1)x));
        }

        protected abstract ICeasableFlow<IOption<ROut>> Evaluate(IRuntime runtime, IOption<RArg1> in1);
        protected Function(IToken<RArg1> in1) : base(in1) { }
    }

    /// <summary>
    /// Core.Tokens that inherit must have a constructor matching: <br></br>
    /// <code>(IToken&lt;<typeparamref name="RArg1"/>&gt;, IToken&lt;<typeparamref name="RArg2"/>&gt;)</code>
    /// </summary>
    /// <typeparam name="RArg1"></typeparam>
    /// <typeparam name="RArg2"></typeparam>
    public abstract record Function<RArg1, RArg2, ROut> : Token<ROut>,
        IFunction<RArg1, RArg2, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        public IToken<RArg2> Arg2 => (IToken<RArg2>)ArgTokens[1];
        public sealed override ICeasableFlow<IOption<ROut>> Resolve(IRuntime runtime, IOption<ResObj>[] args)
        {
            return Evaluate(runtime, args[0].RemapAs(x => (RArg1)x), args[1].RemapAs(x => (RArg2)x));
        }

        protected abstract ICeasableFlow<IOption<ROut>> Evaluate(IRuntime runtime, IOption<RArg1> in1, IOption<RArg2> in2);
        protected Function(IToken<RArg1> in1, IToken<RArg2> in2) : base(in1, in2) { }
    }

    /// <summary>
    /// Core.Tokens that inherit must have a constructor matching: <br></br>
    /// <code>(IToken&lt;<typeparamref name="RArg1"/>&gt;, IToken&lt;<typeparamref name="RArg2"/>&gt;, IToken&lt;<typeparamref name="RArg3"/>&gt;)</code>
    /// </summary>
    /// <typeparam name="RArg1"></typeparam>
    /// <typeparam name="RArg2"></typeparam>
    /// <typeparam name="RArg3"></typeparam>
    public abstract record Function<RArg1, RArg2, RArg3, ROut> : Token<ROut>,
        IFunction<RArg1, RArg2, RArg3, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        public IToken<RArg2> Arg2 => (IToken<RArg2>)ArgTokens[1];
        public IToken<RArg3> Arg3 => (IToken<RArg3>)ArgTokens[2];
        public sealed override ICeasableFlow<IOption<ROut>> Resolve(IRuntime runtime, IOption<ResObj>[] args)
        {
            return Evaluate(runtime, args[0].RemapAs(x => (RArg1)x), args[1].RemapAs(x => (RArg2)x), args[2].RemapAs(x => (RArg3)x));
        }

        protected abstract ICeasableFlow<IOption<ROut>> Evaluate(IRuntime runtime, IOption<RArg1> in1, IOption<RArg2> in2, IOption<RArg3> in3);
        protected Function(IToken<RArg1> in1, IToken<RArg2> in2, IToken<RArg3> in3) : base(in1, in2, in3) { }
        
    }

    /// <summary>
    /// Core.Tokens that inherit must have a constructor matching: <br></br>
    /// <code>(IEnumerable&lt;IToken&lt;<typeparamref name="RArg"/>&gt;&gt;)</code>
    /// </summary>
    /// <typeparam name="RArg"></typeparam>
    public abstract record Combiner<RArg, ROut> : Token<ROut>, ICombiner<RArg, ROut>
        where RArg : class, ResObj
        where ROut : class, ResObj
    {
        public IEnumerable<IToken<RArg>> Args => ArgTokens.Map(x => (IToken<RArg>)x);
        public sealed override ICeasableFlow<IOption<ROut>> Resolve(IRuntime runtime, IOption<ResObj>[] tokens)
        {
            return Evaluate(runtime, tokens.Map(x => x.RemapAs(x => (RArg)x)));
        }

        protected abstract ICeasableFlow<IOption<ROut>> Evaluate(IRuntime runtime, IEnumerable<IOption<RArg>> inputs);
        protected Combiner(IEnumerable<IToken<RArg>> tokens) : base(tokens) { }

    }
    #region Pure Functions
    // -- [ Pure Functions ] --
    public abstract record PureFunction<RArg1, ROut> : Function<RArg1, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {

        protected abstract ROut EvaluatePure(RArg1 in1);
        protected PureFunction(IToken<RArg1> in1) : base(in1) { }
        protected sealed override ICeasableFlow<IOption<ROut>> Evaluate(IRuntime _, IOption<RArg1> in1)
        {
            IOption<ROut> o = (in1.CheckNone(out var a)) ? new None<ROut>() :
                EvaluatePure(a).AsSome();
            return ControlledFlow.Resolved(o);
        }
    }
    public abstract record PureFunction<RArg1, RArg2, ROut> : Function<RArg1, RArg2, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        protected abstract ROut EvaluatePure(RArg1 in1, RArg2 in2);
        protected PureFunction(IToken<RArg1> in1, IToken<RArg2> in2) : base(in1, in2) { }
        protected sealed override ICeasableFlow<IOption<ROut>> Evaluate(IRuntime _, IOption<RArg1> in1, IOption<RArg2> in2)
        {
            IOption<ROut> o = (in1.CheckNone(out var a) || in2.CheckNone(out var b)) ? new None<ROut>() :
                EvaluatePure(a, b).AsSome();
            return ControlledFlow.Resolved(o);
        }
    }
    public abstract record PureFunction<RArg1, RArg2, RArg3, ROut> : Function<RArg1, RArg2, RArg3, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {

        protected abstract ROut EvaluatePure(RArg1 in1, RArg2 in2, RArg3 in3);
        protected PureFunction(IToken<RArg1> in1, IToken<RArg2> in2, IToken<RArg3> in3) : base(in1, in2, in3) { }
        protected sealed override ICeasableFlow<IOption<ROut>> Evaluate(IRuntime _, IOption<RArg1> in1, IOption<RArg2> in2, IOption<RArg3> in3)
        {
            IOption<ROut> o = (in1.CheckNone(out var a) || in2.CheckNone(out var b) || in3.CheckNone(out var c)) ? new None<ROut>() :
                EvaluatePure(a, b, c).AsSome();
            return ControlledFlow.Resolved(o);
        }
    }
    public abstract record PureCombiner<RArg, ROut> : Combiner<RArg, ROut>
        where RArg : class, ResObj
        where ROut : class, ResObj
    {

        protected abstract ROut EvaluatePure(IEnumerable<RArg> inputs);
        protected PureCombiner(IEnumerable<IToken<RArg>> tokens) : base(tokens) { }
        protected sealed override ICeasableFlow<IOption<ROut>> Evaluate(IRuntime _, IEnumerable<IOption<RArg>> inputs) => ControlledFlow.Resolved(EvaluatePure(inputs.Where(x => x.IsSome()).Map(x => x.Unwrap())).AsSome());
    }
    // ----
    #endregion
    // --------
    #endregion

    public abstract record PresentStateGetter<RSource> : Function<RSource, RSource>
        where RSource : class, Resolution.IStateTracked
    {
        public PresentStateGetter(IToken<RSource> source) : base(source) { }
        protected abstract PIndexedSet<int, RSource> GetStatePSet(IRuntime runtime);
        protected sealed override ICeasableFlow<IOption<RSource>> Evaluate(IRuntime runtime, IOption<RSource> in1) { return ControlledFlow.Resolved(in1.RemapAs(x => GetStatePSet(runtime)[x.UUID])); }
    }
}
namespace FourZeroOne.Token.Unsafe
{
    using ResObj = Resolution.IResolution;
    using Token;
    using Runtime;
    public interface IToken
    {
        public IToken[] ArgTokens { get; }
        public ICeasableFlow<IOption<ResObj>> ResolveUnsafe(IRuntime runtime, IOption<ResObj>[] args);
    }

    public interface IHasArg1<RArg> : IHasArg1 where RArg : class, ResObj
    { public IToken<RArg> Arg1 { get; } }
    public interface IHasArg2<RArg> : IHasArg2 where RArg : class, ResObj
    { public IToken<RArg> Arg2 { get; } }
    public interface IHasArg3<RArg> : IHasArg3 where RArg : class, ResObj
    { public IToken<RArg> Arg3 { get; } }
    public interface IHasCombinerArgs<RArgs> : IToken where RArgs : class, ResObj
    {
        public IEnumerable<IToken<RArgs>> Args { get; }
    }

    // shitty name for this interface-set. consider something like 'FromArgs', 'ProductOfArgs', 'ArgTransformer', or something.
    public interface IFunction<ROut> : IFunction, IToken<ROut> where ROut : class, ResObj { }
    public interface IFunction : IToken { }

    public interface IHasArg1 : IToken { }
    public interface IHasArg2 : IHasArg1 { }
    public interface IHasArg3 : IHasArg2 { }

    public abstract record VariableIdentifier
    {
        public VariableIdentifier()
        {
            _value = _assigner;
            _assigner++;
        }
        public virtual bool Equals(VariableIdentifier? other) => other is not null && _value == other._value;
        public override int GetHashCode() => _value.GetHashCode();
        protected static int _assigner = 0;
        protected readonly int _value;
    }

}