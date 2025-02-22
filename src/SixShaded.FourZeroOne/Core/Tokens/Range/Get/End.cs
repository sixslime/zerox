#nullable enable

namespace SixShaded.FourZeroOne.Core.Tokens.Range.Get
{
    using Resolutions;
    public sealed record End : Token.Defined.PureFunction<NumRange, Number>
    {
        public End(IToken<NumRange> range) : base(range) { }
        protected override Number EvaluatePure(NumRange in1)
        {
            return in1.End;
        }
    }
}