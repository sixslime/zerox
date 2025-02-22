#nullable enable
namespace SixShaded.FourZeroOne.Core.Resolutions
{
    // not even a resolution, consider moving.
    public record MergeSpec<C> : ICompositionType where C : ICompositionType
    {
        public static MergeComponentIdentifier<R> MERGE<R>(IComponentIdentifier<C, R> component) where R : class, Res => new(component);

        public interface IMergeIdentifier
        {
            public Resolution.Unsafe.IComponentIdentifier<C> ForComponentUnsafe { get; }
        }
        public record MergeComponentIdentifier<R> : IMergeIdentifier, IComponentIdentifier<MergeSpec<C>, R> where R : class, Res
        {
            public IComponentIdentifier<C, R> ForComponent { get; private init; }
            public Resolution.Unsafe.IComponentIdentifier<C> ForComponentUnsafe => ForComponent;
            public string Package => "CORE";
            public string Identity => $"merge-{ForComponent.Identity}";
            public MergeComponentIdentifier(IComponentIdentifier<C, R> component)
            {
                ForComponent = component;
            }
            public override string ToString() => $"{ForComponent.Identity}*";
        }
    }
}