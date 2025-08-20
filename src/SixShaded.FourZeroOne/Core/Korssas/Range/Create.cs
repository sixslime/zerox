namespace SixShaded.FourZeroOne.Core.Korssas.Range;

using Roggis;

public sealed record Create : Korssa.Defined.Function<Number, Number, NumRange>
{
    public Create(IKorssa<Number> min, IKorssa<Number> max) : base(min, max)
    { }

    protected override ITask<IOption<NumRange>> Evaluate(IKorssaContext runtime, IOption<Number> in1, IOption<Number> in2) =>
        ((in1.Check(out var start) && in2.Check(out var end))
            ? (start.Value <= end.Value)
                ? new NumRange()
                {
                    Start = start,
                    End = end
                }.AsSome()
                : new None<NumRange>()
            : new None<NumRange>())
        .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"{Arg1}..{Arg2}".AsSome();
}