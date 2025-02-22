#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Resolutions
{
    public record MergeSpec<C> : ICompositionType where C : ICompositionType
    {
        public static MergeComponentIdentifier<C, R> MERGE<R>(IComponentIdentifier<C, R> component) where R : Res => new(component);

        public interface IMergeIdentifier
        {
            public IComponentIdentifier<C> ForComponentUnsafe { get; }
        }
        public record MergeComponentIdentifier<R> : IMergeIdentifier, IComponentIdentifier<MergeSpec<C>, R> where R : Res
        {
            public IComponentIdentifier<C, R> ForComponent { get; private init; }
            public IComponentIdentifier<C> ForComponentUnsafe => ForComponent;
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