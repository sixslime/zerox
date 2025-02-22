#nullable enable
namespace SixShaded.FourZeroOne.Resolution
{
    public interface IComponentIdentifier<in C, in R> : Unsafe.IComponentIdentifier<C> where C : ICompositionType where R : Res { }

}