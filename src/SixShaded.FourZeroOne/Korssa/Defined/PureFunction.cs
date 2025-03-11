namespace SixShaded.FourZeroOne.Korssa.Defined;

public abstract record PureFunction<RArg1, ROut> : Function<RArg1, ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    protected PureFunction(IKorssa<RArg1> in1) : base(in1)
    { }

    protected abstract ROut EvaluatePure(RArg1 in1);

    protected sealed override ITask<IOption<ROut>> Evaluate(IKorssaContext _, IOption<RArg1> in1)
    {
        var o = in1.CheckNone(out var a) ? new None<ROut>() : EvaluatePure(a).AsSome();
        return o.ToCompletedITask();
    }
}

public abstract record PureFunction<RArg1, RArg2, ROut> : Function<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    protected PureFunction(IKorssa<RArg1> in1, IKorssa<RArg2> in2) : base(in1, in2)
    { }

    protected abstract ROut EvaluatePure(RArg1 in1, RArg2 in2);

    protected sealed override ITask<IOption<ROut>> Evaluate(IKorssaContext _, IOption<RArg1> in1, IOption<RArg2> in2)
    {
        var o = in1.CheckNone(out var a) || in2.CheckNone(out var b) ? new None<ROut>() : EvaluatePure(a, b).AsSome();
        return o.ToCompletedITask();
    }
}

public abstract record PureFunction<RArg1, RArg2, RArg3, ROut> : Function<RArg1, RArg2, RArg3, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    protected PureFunction(IKorssa<RArg1> in1, IKorssa<RArg2> in2, IKorssa<RArg3> in3) : base(in1, in2, in3)
    { }

    protected abstract ROut EvaluatePure(RArg1 in1, RArg2 in2, RArg3 in3);

    protected sealed override ITask<IOption<ROut>> Evaluate(IKorssaContext _, IOption<RArg1> in1, IOption<RArg2> in2, IOption<RArg3> in3)
    {
        var o = in1.CheckNone(out var a) || in2.CheckNone(out var b) || in3.CheckNone(out var c) ? new None<ROut>() : EvaluatePure(a, b, c).AsSome();
        return o.ToCompletedITask();
    }
}