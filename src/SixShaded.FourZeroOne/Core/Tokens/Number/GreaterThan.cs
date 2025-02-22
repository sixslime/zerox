namespace SixShaded.FourZeroOne.Core.Tokens.Number;

using Resolutions;
public sealed record GreaterThan : Token.Defined.PureFunction<Number, Number, Bool>
{
    public GreaterThan(IToken<Number> a, IToken<Number> b) : base(a, b) { }
    protected override Bool EvaluatePure(Number in1, Number in2) => new() { IsTrue = in1.Value > in2.Value };
    protected override IOption<string> CustomToString() => $"({Arg1} > {Arg2})".AsSome();
}