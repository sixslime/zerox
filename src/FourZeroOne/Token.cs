using FourZeroOne.Resolution;
using MorseCode.ITask;
#nullable enable
namespace FourZeroOne.Token
{
    using Handles;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;
    using any_token = IToken<IResolution>;
    using ResObj = Resolution.IResolution;

    public interface IToken<out R> where R : class, ResObj
    {
        public any_token[] ArgTokens { get; }
        public IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> ResolveWith(FZOSpec.IProcessorFZO.ITokenContext runtime, IOption<ResObj>[] args);
    }
    public abstract record TokenBehavior<R> : IToken<R> where R : class, ResObj
    {
        public any_token[] ArgTokens { get; }
        public TokenBehavior(params any_token[] args)
        {
            ArgTokens = args;
        }
        public TokenBehavior(IEnumerable<any_token> args) : this(args.ToArray()) { }
        public IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> ResolveWith(FZOSpec.IProcessorFZO.ITokenContext tokenContext, IOption<ResObj>[] args) { return Resolve(tokenContext.ToHandle(), args); }
        protected abstract IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> Resolve(ITokenContext runtime, IOption<ResObj>[] args);
        protected virtual IOption<string> CustomToString() => new None<string>();
        public sealed override string ToString()
        {
            return CustomToString()
                .OrElse(() => $"{this.GetType().Name}( {ArgTokens.AccumulateInto("", (msg, arg) => $"{msg}{arg} ")})");
        }
    }
    public abstract record StandardToken<R> : TokenBehavior<R> where R : class, ResObj
    {
        protected abstract ITask<IOption<R>> StandardResolve(ITokenContext runtime, IOption<ResObj>[] args);
        protected override IResult<ITask<IOption<R>>, FZOSpec.EStateImplemented> Resolve(ITokenContext runtime, IOption<ResObj>[] args)
        {
            return new Ok<ITask<IOption<R>>, FZOSpec.EStateImplemented>(StandardResolve(runtime, args));
        }
        public StandardToken(params any_token[] args) : base(args) { }
        public StandardToken(IEnumerable<any_token> args) : base(args) { }
    }

    public interface IHasNoArgs<out RVal> : IToken<RVal>
        where RVal : class, ResObj
    { }
    public interface IHasArgs<out RArg1, out ROut> : IToken<ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    { public IToken<RArg1> Arg1 { get; } }
    public interface IHasArgs<out RArg1, out RArg2, out ROut> : IHasArgs<RArg1, ROut>
    where RArg1 : class, ResObj
    where RArg2 : class, ResObj
    where ROut : class, ResObj
    { public IToken<RArg2> Arg2 { get; } }
    public interface IHasArgs<out RArg1, out RArg2, out RArg3, out ROut> : IHasArgs<RArg1, RArg2, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    { public IToken<RArg3> Arg3 { get; } }
    public interface IHasCombinerArgs<out RArgs, out ROut> : IToken<ROut>
    where RArgs : class, ResObj
    where ROut : class, ResObj
    { public IEnumerable<IToken<RArgs>> Args { get; } }
    
    public interface IHasAttachedComponentIdentifier<in C, out R> : IToken<R>
        where C : ICompositionType
        where R : class, ResObj
    {
        public Resolution.Unsafe.IComponentIdentifier<C> AttachedComponentIdentifier { get; }
    }
    public abstract record Value<R> : StandardToken<R>, IHasNoArgs<R>
        where R : class, ResObj
    {
        protected sealed override ITask<IOption<R>> StandardResolve(ITokenContext runtime, IOption<ResObj>[] _)
        {
            return Evaluate(runtime);
        }
        protected Value() : base() { }
        protected abstract ITask<IOption<R>> Evaluate(ITokenContext runtime);
    }
    public abstract record PureValue<R> : Value<R>
        where R : class, ResObj
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
    public abstract record Function<RArg1, ROut> : StandardToken<ROut>,
        IHasArgs<RArg1, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        protected sealed override ITask<IOption<ROut>> StandardResolve(ITokenContext runtime, IOption<ResObj>[] args)
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
    public abstract record Function<RArg1, RArg2, ROut> : StandardToken<ROut>,
        IHasArgs<RArg1, RArg2, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        public IToken<RArg2> Arg2 => (IToken<RArg2>)ArgTokens[1];
        protected sealed override ITask<IOption<ROut>> StandardResolve(ITokenContext runtime, IOption<ResObj>[] args)
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
    public abstract record Function<RArg1, RArg2, RArg3, ROut> : StandardToken<ROut>,
        IHasArgs<RArg1, RArg2, RArg3, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        public IToken<RArg2> Arg2 => (IToken<RArg2>)ArgTokens[1];
        public IToken<RArg3> Arg3 => (IToken<RArg3>)ArgTokens[2];
        protected sealed override ITask<IOption<ROut>> StandardResolve(ITokenContext runtime, IOption<ResObj>[] args)
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
    public abstract record Combiner<RArg, ROut> : StandardToken<ROut>, IHasCombinerArgs<RArg, ROut>
        where RArg : class, ResObj
        where ROut : class, ResObj
    {
        public IEnumerable<IToken<RArg>> Args => ArgTokens.Map(x => (IToken<RArg>)x);
        protected sealed override ITask<IOption<ROut>> StandardResolve(ITokenContext runtime, IOption<ResObj>[] tokens)
        {
            return Evaluate(runtime, tokens.Map(x => x.RemapAs(x => (RArg)x)));
        }

        protected abstract ITask<IOption<ROut>> Evaluate(ITokenContext runtime, IEnumerable<IOption<RArg>> inputs);
        protected Combiner(IEnumerable<IToken<RArg>> tokens) : base(tokens) { }

    }
    public abstract record RuntimeHandledValue<ROut> : TokenBehavior<ROut>,
        IHasNoArgs<ROut>
        where ROut : class, ResObj
    {
        protected sealed override IResult<ITask<IOption<ROut>>, FZOSpec.EStateImplemented> Resolve(ITokenContext _, IOption<ResObj>[] args)
        {
            return new Err<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(MakeData());
        }
        protected abstract FZOSpec.EStateImplemented MakeData();
        protected RuntimeHandledValue() : base() { }
    }
    public abstract record RuntimeHandledFunction<RArg1, ROut> : TokenBehavior<ROut>,
        IHasArgs<RArg1, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        protected sealed override IResult<ITask<IOption<ROut>>, FZOSpec.EStateImplemented> Resolve(ITokenContext _, IOption<ResObj>[] args)
        {
            return (args[0].Check(out var in1))
                ? new Err<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(MakeData((RArg1)in1))
                : new Ok<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(new None<ROut>().ToCompletedITask());
        }
        protected abstract FZOSpec.EStateImplemented MakeData(RArg1 in1);
        protected RuntimeHandledFunction(IToken<RArg1> in1) : base(in1) { }
    }
    public abstract record RuntimeHandledFunction<RArg1, RArg2, ROut> : TokenBehavior<ROut>,
        IHasArgs<RArg1, RArg2, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        public IToken<RArg2> Arg2 => (IToken<RArg2>)ArgTokens[1];
        protected sealed override IResult<ITask<IOption<ROut>>, FZOSpec.EStateImplemented> Resolve(ITokenContext _, IOption<ResObj>[] args)
        {
            return (args[0].Check(out var in1) && args[1].Check(out var in2))
                ? new Err<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(MakeData((RArg1)in1, (RArg2)in2))
                : new Ok<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(new None<ROut>().ToCompletedITask());
        }
        protected abstract FZOSpec.EStateImplemented MakeData(RArg1 in1, RArg2 in2);
        protected RuntimeHandledFunction(IToken<RArg1> in1, IToken<RArg2> in2) : base(in1, in2) { }
    }
    public abstract record RuntimeHandledFunction<RArg1, RArg2, RArg3, ROut> : TokenBehavior<ROut>,
        IHasArgs<RArg1, RArg2, ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        public IToken<RArg2> Arg2 => (IToken<RArg2>)ArgTokens[1];
        public IToken<RArg3> Arg3 => (IToken<RArg3>)ArgTokens[2];
        protected sealed override IResult<ITask<IOption<ROut>>, FZOSpec.EStateImplemented> Resolve(ITokenContext _, IOption<ResObj>[] args)
        {
            return (args[0].Check(out var in1) && args[1].Check(out var in2) && args[2].Check(out var in3))
                ? new Err<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(MakeData((RArg1)in1, (RArg2)in2, (RArg3)in3))
                : new Ok<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(new None<ROut>().ToCompletedITask());
        }
        protected abstract FZOSpec.EStateImplemented MakeData(RArg1 in1, RArg2 in2, RArg3 in3);
        protected RuntimeHandledFunction(IToken<RArg1> in1, IToken<RArg2> in2, IToken<RArg3> in3) : base(in1, in2, in3) { }
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