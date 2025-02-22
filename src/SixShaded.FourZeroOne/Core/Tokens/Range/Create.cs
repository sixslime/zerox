#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Range
{
    public sealed record Create : PureFunction<ro.Number, ro.Number, ro.NumRange>
    {
        public Create(IToken<ro.Number> min, IToken<ro.Number> max) : base(min, max) { }
        protected override ro.NumRange EvaluatePure(ro.Number in1, ro.Number in2)
        {
            return new() { Start = in1, End = in2 };
        }
    }
}