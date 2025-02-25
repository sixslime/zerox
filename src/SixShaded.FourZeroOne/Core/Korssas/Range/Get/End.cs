namespace SixShaded.FourZeroOne.Core.Korssas.Range.Get;

using Roggis;

public sealed record End : Korssa.Defined.PureFunction<NumRange, Number>
{
    public End(IKorssa<NumRange> range) : base(range) { }
    protected override Number EvaluatePure(NumRange in1) => in1.End;
}