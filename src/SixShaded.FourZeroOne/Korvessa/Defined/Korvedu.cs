namespace SixShaded.FourZeroOne.Korvessa.Defined;

public sealed class Korvedu(Axodu axodu, string identifier) : Axovendu(axodu, identifier)
{
    protected override string TypeExpression { get; } = "Korvedu";
    public string GetSignature() => $"{Axodu.Name}.{Identifier}";
}