namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

public sealed record Yield<R> : Korssa.Defined.PureFunction<R, Roggis.Multi<R>>
    where R : class, Rog
{
    public Yield(IKorssa<R> value) : base(value)
    { }

    protected override Roggis.Multi<R> EvaluatePure(R in1) =>
        new()
        {
            Values = in1.Yield().ToPSequence(),
        };

    protected override IOption<string> CustomToString() => $"^{Arg1}".AsSome();
}