#nullable enable
namespace SixShaded.FourZeroOne.Resolution.Unsafe
{
    using FourZeroOne.FZOSpec;
    using Handles;
    using SixShaded.FourZeroOne;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;

    public interface IComponentIdentifier<in C> : IComponentIdentifier where C : ICompositionType { }
    public interface IComponentIdentifier
    {
        public string Identity { get; }
        public string Package { get; }
    }
}
