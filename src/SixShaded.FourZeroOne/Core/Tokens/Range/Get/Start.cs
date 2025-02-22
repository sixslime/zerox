#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Range.Get
{
    public sealed record Start : PureFunction<ro.NumRange, ro.Number>
    {
        public Start(IToken<ro.NumRange> range) : base(range) { }
        protected override ro.Number EvaluatePure(ro.NumRange in1)
        {
            return in1.Start;
        }
    }
}