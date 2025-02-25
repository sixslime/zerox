namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record RuntimeHandledFunction<RArg1, ROut> : Korssa<ROut>,
    IHasArgs<RArg1, ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    protected RuntimeHandledFunction(IKorssa<RArg1> in1) : base(in1) { }
    public IKorssa<RArg1> Arg1 => (IKorssa<RArg1>)ArgKorssas[0];

    protected sealed override IResult<ITask<IOption<ROut>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext _, RogOpt[] args) =>
        args[0].Check(out var in1)
            ? new Err<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(MakeData((RArg1)in1))
            : new Ok<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(new None<ROut>().ToCompletedITask());

    protected abstract FZOSpec.EStateImplemented MakeData(RArg1 in1);
}

public abstract record RuntimeHandledFunction<RArg1, RArg2, ROut> : Korssa<ROut>,
    IHasArgs<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    protected RuntimeHandledFunction(IKorssa<RArg1> in1, IKorssa<RArg2> in2) : base(in1, in2) { }
    public IKorssa<RArg1> Arg1 => (IKorssa<RArg1>)ArgKorssas[0];
    public IKorssa<RArg2> Arg2 => (IKorssa<RArg2>)ArgKorssas[1];

    protected sealed override IResult<ITask<IOption<ROut>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext _, RogOpt[] args) =>
        args[0].Check(out var in1) && args[1].Check(out var in2)
            ? new Err<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(MakeData((RArg1)in1, (RArg2)in2))
            : new Ok<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(new None<ROut>().ToCompletedITask());

    protected abstract FZOSpec.EStateImplemented MakeData(RArg1 in1, RArg2 in2);
}

public abstract record RuntimeHandledFunction<RArg1, RArg2, RArg3, ROut> : Korssa<ROut>,
    IHasArgs<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    protected RuntimeHandledFunction(IKorssa<RArg1> in1, IKorssa<RArg2> in2, IKorssa<RArg3> in3) : base(in1, in2, in3) { }
    public IKorssa<RArg3> Arg3 => (IKorssa<RArg3>)ArgKorssas[2];
    public IKorssa<RArg1> Arg1 => (IKorssa<RArg1>)ArgKorssas[0];
    public IKorssa<RArg2> Arg2 => (IKorssa<RArg2>)ArgKorssas[1];

    protected sealed override IResult<ITask<IOption<ROut>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext _, RogOpt[] args) =>
        args[0].Check(out var in1) && args[1].Check(out var in2) && args[2].Check(out var in3)
            ? new Err<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(MakeData((RArg1)in1, (RArg2)in2, (RArg3)in3))
            : new Ok<ITask<IOption<ROut>>, FZOSpec.EStateImplemented>(new None<ROut>().ToCompletedITask());

    protected abstract FZOSpec.EStateImplemented MakeData(RArg1 in1, RArg2 in2, RArg3 in3);
}