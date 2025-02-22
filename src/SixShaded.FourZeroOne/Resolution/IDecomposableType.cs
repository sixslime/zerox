#nullable enable
namespace SixShaded.FourZeroOne.Resolution
{
    public interface IDecomposableType<Self, R> : ICompositionType where Self : IDecomposableType<Self, R>, new() where R : class, Res
    {
        public Core.Resolutions.MetaFunction<ICompositionOf<Self>, R> DecompositionFunction { get; }
    }
}