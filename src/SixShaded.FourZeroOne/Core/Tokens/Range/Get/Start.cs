#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Range.Get
{
    public sealed record Start : PureFunction<NumRange, Number>
    {
        public Start(IToken<NumRange> range) : base(range) { }
        protected override Number EvaluatePure(NumRange in1)
        {
            return in1.Start;
        }
    }
}