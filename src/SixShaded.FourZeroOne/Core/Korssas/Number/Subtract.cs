namespace SixShaded.FourZeroOne.Core.Korssas.Number;

using Roggis;

public sealed record Subtract : Korssa.Defined.PureFunction<Number, Number, Number>
{
    public Subtract(IKorssa<Number> operand1, IKorssa<Number> operand2) : base(operand1, operand2)
    { }

    protected override Number EvaluatePure(Number a, Number b) =>
        new()
        {
            Value = a.Value - b.Value,
        };

    protected override IOption<string> CustomToString() => $"({Arg1} - {Arg2})".AsSome();
}