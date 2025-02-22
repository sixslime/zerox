#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Resolution.Defined
{
    using FourZeroOne.FZOSpec;
    using Handles;
    using SixShaded.FourZeroOne;
    using SixShaded.FourZeroOne.Resolution;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;

    public record StaticComponentIdentifier<H, R> : IComponentIdentifier<H, R> where H : ICompositionType where R : class, IResolution
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
