#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Range.Get
{
    public sealed record End : PureFunction<ro.NumRange, ro.Number>
    {
        public End(IToken<ro.NumRange> range) : base(range) { }
        protected override ro.Number EvaluatePure(ro.NumRange in1)
        {
            return in1.End;
        }
    }
}