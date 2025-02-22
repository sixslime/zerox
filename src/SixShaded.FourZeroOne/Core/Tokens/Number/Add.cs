#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Number
{
    public sealed record Add : PureFunction<ro.Number, ro.Number, ro.Number>
    {
        public Add(IToken<ro.Number> operand1, IToken<ro.Number> operand2) : base(operand1, operand2) { }
        protected override ro.Number EvaluatePure(ro.Number a, ro.Number b) { return new() { Value = a.Value + b.Value }; }
        protected override IOption<string> CustomToString() => $"({Arg1} + {Arg2})".AsSome();
    }
}