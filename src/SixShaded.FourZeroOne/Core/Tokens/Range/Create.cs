#nullable enable
namespace SixShaded.FourZeroOne.Core.Tokens.Range
{
    using Resolutions;
    public sealed record Create : Token.Defined.PureFunction<Number, Number, NumRange>
    {
        public Create(IToken<Number> min, IToken<Number> max) : base(min, max) { }
        protected override NumRange EvaluatePure(Number in1, Number in2)
        {
            return new() { Start = in1, End = in2 };
        }
    }
}