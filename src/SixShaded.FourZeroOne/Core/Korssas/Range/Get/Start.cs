namespace SixShaded.FourZeroOne.Core.Korssas.Range.Get;

using Roggis;

public sealed record Start : Korssa.Defined.PureFunction<NumRange, Number>
{
    public Start(IKorssa<NumRange> range) : base(range)
    { }

    protected override Number EvaluatePure(NumRange in1) => in1.Start;
    protected override IOption<string> CustomToString() => $"{Arg1}<".AsSome();
}