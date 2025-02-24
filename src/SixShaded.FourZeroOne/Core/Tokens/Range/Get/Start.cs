namespace SixShaded.FourZeroOne.Core.Tokens.Range.Get;
using Resolutions;
public sealed record Start : Token.Defined.PureFunction<NumRange, Number>
{
    public Start(IToken<NumRange> range) : base(range) { }
    protected override Number EvaluatePure(NumRange in1) => in1.Start;
}