namespace SixShaded.FourZeroOne.Core.Korssas;

public sealed record Fixed<R> : Korssa.Defined.PureValue<R>
    where R : class, Rog
{
    public readonly R Roggi;

    public Fixed(R roggi)
    {
        Roggi = roggi;
    }

    protected override R EvaluatePure() => Roggi;
    protected override IOption<string> CustomToString() => $"|{Roggi}|".AsSome();
}