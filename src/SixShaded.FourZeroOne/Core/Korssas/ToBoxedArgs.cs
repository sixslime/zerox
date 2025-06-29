namespace SixShaded.FourZeroOne.Core.Korssas;

using Roggis;

public record ToBoxedArgs<R1> : Korssa.Defined.Function<R1, MetaArgs<R1>>
    where R1 : class, Rog
{
    public ToBoxedArgs(IKorssa<R1> in1) : base(in1)
    { }

    protected override ITask<IOption<MetaArgs<R1>>> Evaluate(IKorssaContext _, IOption<R1> in1) =>
        new MetaArgs<R1>
            {
                Arg1 = in1,
            }.AsSome()
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"<{Arg1}>".AsSome();
}

public record ToBoxedArgs<R1, R2> : Korssa.Defined.Function<R1, R2, MetaArgs<R1, R2>>
    where R1 : class, Rog
    where R2 : class, Rog
{
    public ToBoxedArgs(IKorssa<R1> in1, IKorssa<R2> in2) : base(in1, in2)
    { }

    protected override ITask<IOption<MetaArgs<R1, R2>>> Evaluate(IKorssaContext _, IOption<R1> in1, IOption<R2> in2) =>
        new MetaArgs<R1, R2>
            {
                Arg1 = in1,
                Arg2 = in2,
            }.AsSome()
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"<{Arg1}, {Arg2}>".AsSome();
}

public record ToBoxedArgs<R1, R2, R3> : Korssa.Defined.Function<R1, R2, R3, MetaArgs<R1, R2, R3>>
    where R1 : class, Rog
    where R2 : class, Rog
    where R3 : class, Rog
{
    public ToBoxedArgs(IKorssa<R1> in1, IKorssa<R2> in2, IKorssa<R3> in3) : base(in1, in2, in3)
    { }

    protected override ITask<IOption<MetaArgs<R1, R2, R3>>> Evaluate(IKorssaContext _, IOption<R1> in1, IOption<R2> in2, IOption<R3> in3) =>
        new MetaArgs<R1, R2, R3>
            {
                Arg1 = in1,
                Arg2 = in2,
                Arg3 = in3,
            }.AsSome()
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"<{Arg1}, {Arg2}, {Arg3}>".AsSome();
}