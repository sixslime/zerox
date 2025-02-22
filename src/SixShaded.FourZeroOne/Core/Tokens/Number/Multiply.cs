#nullable enable
namespace SixShaded.FourZeroOne.Core.Tokens.Number
{
    using Resolutions;
    public sealed record Multiply : Token.Defined.PureFunction<Number, Number, Number>
    {
        public Multiply(IToken<Number> operand1, IToken<Number> operand2) : base(operand1, operand2) { }
        protected override Number EvaluatePure(Number a, Number b) { return new() { Value = a.Value * b.Value }; }
        protected override IOption<string> CustomToString() => $"({Arg1} * {Arg2})".AsSome();
    }
}