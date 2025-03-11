namespace SixShaded.FourZeroOne.Core.Korssas.Number;

using Roggis;

public sealed record GreaterThan : Korssa.Defined.PureFunction<Number, Number, Bool>
{
    public GreaterThan(IKorssa<Number> a, IKorssa<Number> b) : base(a, b)
    { }

    protected override Bool EvaluatePure(Number in1, Number in2) =>
        new()
        {
            IsTrue = in1.Value > in2.Value,
        };

    protected override IOption<string> CustomToString() => $"({Arg1} > {Arg2})".AsSome();
}