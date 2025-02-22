#nullable enable
namespace SixShaded.FourZeroOne.Token.Defined
{
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
}