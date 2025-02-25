namespace SixShaded.FourZeroOne.Core.Korssas.Range;

using Roggis;

public sealed record Create : Korssa.Defined.PureFunction<Number, Number, NumRange>
{
    public Create(IKorssa<Number> min, IKorssa<Number> max) : base(min, max) { }
    protected override NumRange EvaluatePure(Number in1, Number in2) => new() { Start = in1, End = in2 };
}