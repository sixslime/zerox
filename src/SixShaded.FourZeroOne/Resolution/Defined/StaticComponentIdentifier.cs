#nullable enable
namespace SixShaded.FourZeroOne.Resolution.Defined
{
    public record StaticComponentIdentifier<H, R> : IComponentIdentifier<H, R> where H : ICompositionType where R : class, Res
    {
        public string Package { get; }
        public string Identity { get; }
        public StaticComponentIdentifier(string source, string fixedIdentity)
        {
            Package = source;
            Identity = fixedIdentity;
        }
        public override string ToString() => $"{Identity}";
    }
}
