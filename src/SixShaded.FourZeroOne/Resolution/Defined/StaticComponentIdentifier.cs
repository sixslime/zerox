namespace SixShaded.FourZeroOne.Resolution.Defined;

public record StaticComponentIdentifier<H, R> : IComponentIdentifier<H, R> where H : ICompositionType where R : class, Res
{
    public StaticComponentIdentifier(string source, string fixedIdentity)
    {
        Package = source;
        Identity = fixedIdentity;
    }

    public string Package { get; }
    public string Identity { get; }
    public override string ToString() => $"{Identity}";
}