
using System.Collections.Generic;
using System.Threading.Tasks;
using Perfection;
using ControlledFlows;
using MorseCode.ITask;
using FourZeroOne.Resolution;
#nullable enable
namespace FourZeroOne.Token
{
    using ResObj = Resolution.IResolution;
    using r = Core.Resolutions;
    using PROTO_ZeroxFour_1.Util;
    using Handles;
    public interface IToken<out R> : Unsafe.IToken where R : class, ResObj
    {
        // "ToNodes(IRuntime runtime)".
        // "Resolve(IOption<ResObj>[]...)"
        public IResult<ITask<IOption<R>>, EEvaluatorHandled> Resolve(ITokenContext runtime, IOption<ResObj>[] args);
        public IToken<R> UnsafeTypedWithArgs(Unsafe.IToken[] args);
    }
    public abstract record Token<R> : IToken<R> where R : class, ResObj
    {
        public Unsafe.IToken[] ArgTokens => _argTokens;
        public IPSet<string> Labels { get; private init; } = new PSet<string>();
        public Unsafe.IToken _dLabels(Updater<IPSet<string>> updater) => this with { Labels = updater(Labels) };
        public Token(params Unsafe.IToken[] args)
        {
            _argTokens = args;
            _uniqueId = ++_assigner;
        }
        public Token(IEnumerable<Unsafe.IToken> args) : this(args.ToArray()) { }
        public abstract IResult<ITask<IOption<R>>, EEvaluatorHandled> Resolve(ITokenContext runtime, IOption<ResObj>[] args);
        public IResult<ITask<IOption<ResObj>>, EEvaluatorHandled> UnsafeResolve(Logical.IProcessor.ITokenContext tokenContext, IOption<ResObj>[] args) { return Resolve(tokenContext.ToHandle(), args); }
        // WithArgs() is smelly
        public Unsafe.IToken UnsafeWithArgs(Unsafe.IToken[] args) => UnsafeTypedWithArgs(args);
        public IToken<R> UnsafeTypedWithArgs(Unsafe.IToken[] args) => this with { _argTokens = args };
        protected virtual IOption<string> CustomToString() => new None<string>();

        public sealed override string ToString()
        {
            var mainPart = CustomToString().Check(out var custom)
                ? custom
                : $"{this.GetType().Name}( {_argTokens.AccumulateInto("", (msg, arg) => $"{msg}{arg} ")})";
            var hookPart = Labels.Count > 0
                ? $"-HOOKS[{string.Join(",", Labels.Elements)}]"
                : "";
            return mainPart + hookPart;
        }
        private static int _assigner = 0;
        private Unsafe.IToken[] _argTokens;
        private int _uniqueId;
        
    }
    public abstract record NormalToken<R> : Token<R> where R : class, ResObj
    {
        protected abstract ITask<IOption<R>> NormalResolve(ITokenContext runtime, IOption<ResObj>[] args);
        public override IResult<ITask<IOption<R>>, EEvaluatorHandled> Resolve(ITokenContext runtime, IOption<ResObj>[] args)
        {
            return new Ok<ITask<IOption<R>>, EEvaluatorHandled>(NormalResolve(runtime, args));
        }
        public NormalToken(params Unsafe.IToken[] args) : base(args) { }
        public NormalToken(IEnumerable<Unsafe.IToken> args) : base(args) { }
    }
    public static class SelfAssumptions
    {
        public static Self dLabels<Self>(this Self s, Updater<IPSet<string>> updater) where Self : Unsafe.IToken
            => (Self)s._dLabels(updater);
    }

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

    public abstract record Value<R> : NormalToken<R> where R : class, ResObj
    {
        protected sealed override ITask<IOption<R>> NormalResolve(ITokenContext runtime, IOption<ResObj>[] _)
        {
            return Evaluate(runtime);
        }
        protected Value() : base() { }
        protected abstract ITask<IOption<R>> Evaluate(ITokenContext runtime);
    }
    public abstract record PureValue<R> : Value<R> where R : class, ResObj
    {
        protected PureValue() : base() { }
        protected sealed override ITask<IOption<R>> Evaluate(ITokenContext _)
        {
            return EvaluatePure().AsSome().ToCompletedITask();
        }
        protected abstract R EvaluatePure();
    }
    /// <summary>
    /// Core.Tokens that inherit must have a constructor matching: <br></br>
    /// <code>(IToken&lt;<typeparamref name="RArg1"/>&gt;)</code>
    /// </summary>
    /// <typeparam name="RArg1"></typeparam>
    public abstract record Function<RArg1, ROut> : NormalToken<ROut>,
        IFunction<RArg1, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        protected sealed override ITask<IOption<ROut>> NormalResolve(ITokenContext runtime, IOption<ResObj>[] args)
        {
            return Evaluate(runtime, args[0].RemapAs(x => (RArg1)x));
        }

        protected abstract ITask<IOption<ROut>> Evaluate(ITokenContext runtime, IOption<RArg1> in1);
        protected Function(IToken<RArg1> in1) : base(in1) { }
    }

    /// <summary>
    /// Core.Tokens that inherit must have a constructor matching: <br></br>
    /// <code>(IToken&lt;<typeparamref name="RArg1"/>&gt;, IToken&lt;<typeparamref name="RArg2"/>&gt;)</code>
    /// </summary>
    /// <typeparam name="RArg1"></typeparam>
    /// <typeparam name="RArg2"></typeparam>
    public abstract record Function<RArg1, RArg2, ROut> : NormalToken<ROut>,
        IFunction<RArg1, RArg2, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        public IToken<RArg2> Arg2 => (IToken<RArg2>)ArgTokens[1];
        protected sealed override ITask<IOption<ROut>> NormalResolve(ITokenContext runtime, IOption<ResObj>[] args)
        {
            return Evaluate(runtime, args[0].RemapAs(x => (RArg1)x), args[1].RemapAs(x => (RArg2)x));
        }

        protected abstract ITask<IOption<ROut>> Evaluate(ITokenContext runtime, IOption<RArg1> in1, IOption<RArg2> in2);
        protected Function(IToken<RArg1> in1, IToken<RArg2> in2) : base(in1, in2) { }
    }

    /// <summary>
    /// Core.Tokens that inherit must have a constructor matching: <br></br>
    /// <code>(IToken&lt;<typeparamref name="RArg1"/>&gt;, IToken&lt;<typeparamref name="RArg2"/>&gt;, IToken&lt;<typeparamref name="RArg3"/>&gt;)</code>
    /// </summary>
    /// <typeparam name="RArg1"></typeparam>
    /// <typeparam name="RArg2"></typeparam>
    /// <typeparam name="RArg3"></typeparam>
    public abstract record Function<RArg1, RArg2, RArg3, ROut> : NormalToken<ROut>,
        IFunction<RArg1, RArg2, RArg3, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        public IToken<RArg2> Arg2 => (IToken<RArg2>)ArgTokens[1];
        public IToken<RArg3> Arg3 => (IToken<RArg3>)ArgTokens[2];
        protected sealed override ITask<IOption<ROut>> NormalResolve(ITokenContext runtime, IOption<ResObj>[] args)
        {
            return Evaluate(runtime, args[0].RemapAs(x => (RArg1)x), args[1].RemapAs(x => (RArg2)x), args[2].RemapAs(x => (RArg3)x));
        }

        protected abstract ITask<IOption<ROut>> Evaluate(ITokenContext runtime, IOption<RArg1> in1, IOption<RArg2> in2, IOption<RArg3> in3);
        protected Function(IToken<RArg1> in1, IToken<RArg2> in2, IToken<RArg3> in3) : base(in1, in2, in3) { }
        
    }

    /// <summary>
    /// Core.Tokens that inherit must have a constructor matching: <br></br>
    /// <code>(IEnumerable&lt;IToken&lt;<typeparamref name="RArg"/>&gt;&gt;)</code>
    /// </summary>
    /// <typeparam name="RArg"></typeparam>
    public abstract record Combiner<RArg, ROut> : NormalToken<ROut>, ICombiner<RArg, ROut>
        where RArg : class, ResObj
        where ROut : class, ResObj
    {
        public IEnumerable<IToken<RArg>> Args => ArgTokens.Map(x => (IToken<RArg>)x);
        protected sealed override ITask<IOption<ROut>> NormalResolve(ITokenContext runtime, IOption<ResObj>[] tokens)
        {
            return Evaluate(runtime, tokens.Map(x => x.RemapAs(x => (RArg)x)));
        }

        protected abstract ITask<IOption<ROut>> Evaluate(ITokenContext runtime, IEnumerable<IOption<RArg>> inputs);
        protected Combiner(IEnumerable<IToken<RArg>> tokens) : base(tokens) { }

    }
    public abstract record RuntimeHandledFunction<RArg1, ROut> : Token<ROut>,
        IFunction<RArg1, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        public sealed override IResult<ITask<IOption<ROut>>, EEvaluatorHandled> Resolve(ITokenContext _, IOption<ResObj>[] args)
        {
            return (args[0].Check(out var in1))
                ? new Err<ITask<IOption<ROut>>, EEvaluatorHandled>(MakeData((RArg1)in1))
                : new Ok<ITask<IOption<ROut>>, EEvaluatorHandled>(new None<ROut>().ToCompletedITask());
        }
        protected abstract EEvaluatorHandled MakeData(RArg1 in1);
        protected RuntimeHandledFunction(IToken<RArg1> in1) : base(in1) { }
    }
    public abstract record RuntimeHandledFunction<RArg1, RArg2, ROut> : Token<ROut>,
        IFunction<RArg1, RArg2, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        public IToken<RArg2> Arg2 => (IToken<RArg2>)ArgTokens[1];
        public sealed override IResult<ITask<IOption<ROut>>, EEvaluatorHandled> Resolve(ITokenContext _, IOption<ResObj>[] args)
        {
            return (args[0].Check(out var in1) && args[1].Check(out var in2))
                ? new Err<ITask<IOption<ROut>>, EEvaluatorHandled>(MakeData((RArg1)in1, (RArg2)in2))
                : new Ok<ITask<IOption<ROut>>, EEvaluatorHandled>(new None<ROut>().ToCompletedITask());
        }
        protected abstract EEvaluatorHandled MakeData(RArg1 in1, RArg2 in2);
        protected RuntimeHandledFunction(IToken<RArg1> in1, IToken<RArg2> in2) : base(in1, in2) { }
    }

    public abstract record PureFunction<RArg1, ROut> : Function<RArg1, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {

        protected abstract ROut EvaluatePure(RArg1 in1);
        protected PureFunction(IToken<RArg1> in1) : base(in1) { }
        protected sealed override ITask<IOption<ROut>> Evaluate(ITokenContext _, IOption<RArg1> in1)
        {
            IOption<ROut> o = (in1.CheckNone(out var a)) ? new None<ROut>() :
                EvaluatePure(a).AsSome();
            return o.ToCompletedITask();
        }
    }
    public abstract record PureFunction<RArg1, RArg2, ROut> : Function<RArg1, RArg2, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        protected abstract ROut EvaluatePure(RArg1 in1, RArg2 in2);
        protected PureFunction(IToken<RArg1> in1, IToken<RArg2> in2) : base(in1, in2) { }
        protected sealed override ITask<IOption<ROut>> Evaluate(ITokenContext _, IOption<RArg1> in1, IOption<RArg2> in2)
        {
            IOption<ROut> o = (in1.CheckNone(out var a) || in2.CheckNone(out var b)) ? new None<ROut>() :
                EvaluatePure(a, b).AsSome();
            return o.ToCompletedITask();
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
        protected sealed override ITask<IOption<ROut>> Evaluate(ITokenContext _, IOption<RArg1> in1, IOption<RArg2> in2, IOption<RArg3> in3)
        {
            IOption<ROut> o = (in1.CheckNone(out var a) || in2.CheckNone(out var b) || in3.CheckNone(out var c)) ? new None<ROut>() :
                EvaluatePure(a, b, c).AsSome();
            return o.ToCompletedITask();
        }
    }
    public abstract record PureCombiner<RArg, ROut> : Combiner<RArg, ROut>
        where RArg : class, ResObj
        where ROut : class, ResObj
    {

        protected abstract ROut EvaluatePure(IEnumerable<RArg> inputs);
        protected PureCombiner(IEnumerable<IToken<RArg>> tokens) : base(tokens) { }
        protected sealed override ITask<IOption<ROut>> Evaluate(ITokenContext _, IEnumerable<IOption<RArg>> inputs) => EvaluatePure(inputs.Where(x => x.IsSome()).Map(x => x.Unwrap())).AsSome().ToCompletedITask();
    }

}
namespace FourZeroOne.Token.Unsafe
{
    using ResObj = Resolution.IResolution;
    using Token;
    using Handles;
    public interface IToken
    {
        public IToken[] ArgTokens { get; }
        public IPSet<string> Labels { get; } 
        public IToken _dLabels(Updater<IPSet<string>> updater);
        // SMELL: 'UnsafeResolve()' takes a direct 'ITokenContext' while 'Resolve()' takes a handle.
        public IResult<ITask<IOption<ResObj>>, EEvaluatorHandled> UnsafeResolve(Logical.IProcessor.ITokenContext runtime, IOption<ResObj>[] args);
        public IToken UnsafeWithArgs(IToken[] args);
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
}