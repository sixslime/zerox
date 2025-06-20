namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record StateImplementedKorssa<RVal> : Korssa<RVal>,
    IHasNoArgs<RVal>
    where RVal : class, Rog
{
    protected sealed override IResult<ITask<IOption<RVal>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext context, RogOpt[] args) => MakeData(context).OrElseErr(() => new None<RVal>().ToCompletedITask()).Invert();
    protected abstract IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext context);
}

public abstract record StateImplementedKorssa<RArg1, ROut> : Korssa<ROut>,
    IHasArgs<RArg1, ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    protected StateImplementedKorssa(IKorssa<RArg1> in1) : base(in1)
    { }

    protected sealed override IResult<ITask<IOption<ROut>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext context, RogOpt[] args) => MakeData(context, args[0].RemapAs(x => (RArg1)x)).OrElseErr(() => new None<ROut>().ToCompletedITask()).Invert();

    protected abstract IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext context, IOption<RArg1> in1);
    public IKorssa<RArg1> Arg1 => (IKorssa<RArg1>)ArgKorssas[0];
}

public abstract record StateImplementedKorssa<RArg1, RArg2, ROut> : Korssa<ROut>,
    IHasArgs<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    protected StateImplementedKorssa(IKorssa<RArg1> in1, IKorssa<RArg2> in2) : base(in1, in2)
    { }

    protected sealed override IResult<ITask<IOption<ROut>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext context, RogOpt[] args) => MakeData(context, args[0].RemapAs(x => (RArg1)x), args[1].RemapAs(x => (RArg2)x)).OrElseErr(() => new None<ROut>().ToCompletedITask()).Invert();

    protected abstract IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext context, IOption<RArg1> in1, IOption<RArg2> in2);
    public IKorssa<RArg1> Arg1 => (IKorssa<RArg1>)ArgKorssas[0];
    public IKorssa<RArg2> Arg2 => (IKorssa<RArg2>)ArgKorssas[1];
}

public abstract record StateImplementedKorssa<RArg1, RArg2, RArg3, ROut> : Korssa<ROut>,
    IHasArgs<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    protected StateImplementedKorssa(IKorssa<RArg1> in1, IKorssa<RArg2> in2, IKorssa<RArg3> in3) : base(in1, in2, in3)
    { }

    public IKorssa<RArg3> Arg3 => (IKorssa<RArg3>)ArgKorssas[2];

    protected sealed override IResult<ITask<IOption<ROut>>, FZOSpec.EStateImplemented> Resolve(IKorssaContext context, RogOpt[] args) => MakeData(context, args[0].RemapAs(x => (RArg1)x), args[1].RemapAs(x => (RArg2)x), args[2].RemapAs(x => (RArg3)x)).OrElseErr(() => new None<ROut>().ToCompletedITask()).Invert();

    protected abstract IOption<FZOSpec.EStateImplemented> MakeData(IKorssaContext context, IOption<RArg1> in1, IOption<RArg2> in2, IOption<RArg3> in3);
    public IKorssa<RArg1> Arg1 => (IKorssa<RArg1>)ArgKorssas[0];
    public IKorssa<RArg2> Arg2 => (IKorssa<RArg2>)ArgKorssas[1];
}