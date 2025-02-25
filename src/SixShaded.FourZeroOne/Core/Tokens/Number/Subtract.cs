namespace SixShaded.FourZeroOne.Core.Tokens.Number;

using Resolutions;

public sealed record Subtract : Token.Defined.PureFunction<Number, Number, Number>
{
    public Subtract(IToken<Number> operand1, IToken<Number> operand2) : base(operand1, operand2) { }
    protected override Number EvaluatePure(Number a, Number b) => new() { Value = a.Value - b.Value };
    protected override IOption<string> CustomToString() => $"({Arg1} - {Arg2})".AsSome();
}