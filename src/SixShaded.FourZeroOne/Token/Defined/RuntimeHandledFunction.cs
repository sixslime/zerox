#nullable enable
namespace SixShaded.FourZeroOne.Token.Defined
{
    public abstract record RuntimeHandledFunction<RArg1, ROut> : TokenBehavior<ROut>,
        IHasArgs<RArg1, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public IToken<RArg1> Arg1 => (IToken<RArg1>)ArgTokens[0];
        protected sealed override IResult<ITask<IOption<ROut>>, FZOSpec.EStateImplemented> Resolve(ITokenContext _, IOption<ResObj>[] args)
        {
            return args[0].Check(out var in1)
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
            return args[0].Check(out var in1) && args[1].Check(out var in2)
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
            return args[0].Check(out var in1) && args[1].Check(out var in2) && args[2].Check(out var in3)
                ? new Err<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(MakeData((RArg1)in1, (RArg2)in2, (RArg3)in3))
                : new Ok<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(new None<ROut>().ToCompletedITask());
        }
        protected abstract FZOSpec.EStateImplemented MakeData(RArg1 in1, RArg2 in2, RArg3 in3);
        protected RuntimeHandledFunction(IToken<RArg1> in1, IToken<RArg2> in2, IToken<RArg3> in3) : base(in1, in2, in3) { }
    }
}