#nullable enable
namespace FourZeroOne.Resolution
{
    using FourZeroOne.FZOSpec;
    using Handles;
    using SixShaded.FourZeroOne;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;
    
    public interface IDecomposableType<Self, R> : ICompositionType where Self : IDecomposableType<Self, R>, new() where R : class, IResolution
    {
        public Core.Resolutions.Boxed.MetaFunction<ICompositionOf<Self>, R> DecompositionFunction { get; }
    }
   
}