#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Number
{
    public sealed record GreaterThan : PureFunction<ro.Number, ro.Number, ro.Bool>
    {
        public GreaterThan(IToken<ro.Number> a, IToken<ro.Number> b) : base(a, b) { }
        protected override ro.Bool EvaluatePure(ro.Number in1, ro.Number in2)
        {
            return new() { IsTrue = in1.Value > in2.Value };
        }
        protected override IOption<string> CustomToString() => $"({Arg1} > {Arg2})".AsSome();
    }
}