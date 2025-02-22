namespace SixShaded.FourZeroOne.Core.Tokens;

public sealed record Fixed<R> : Token.Defined.PureValue<R> where R : class, Res
{
    public readonly R Resolution;

    public Fixed(R resolution)
    {
        Resolution = resolution;
    }

    protected override R EvaluatePure() => Resolution;
    protected override IOption<string> CustomToString() => $"|{Resolution}|".AsSome();
}