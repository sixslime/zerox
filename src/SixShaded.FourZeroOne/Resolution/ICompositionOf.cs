
namespace SixShaded.FourZeroOne.Resolution
{
    // pretty silly bro im not going even to even lie even.
    public interface ICompositionOf<out C> : Res where C : ICompositionType
    {
        public IEnumerable<ITiple<Unsafe.IComponentIdentifier, Res>> ComponentsUnsafe { get; }
        public ICompositionOf<C> WithComponent<R>(IComponentIdentifier<C, R> identifier, R data) where R : Res;
        public ICompositionOf<C> WithComponentsUnsafe(IEnumerable<ITiple<Unsafe.IComponentIdentifier<C>, Res>> components);
        public ICompositionOf<C> WithoutComponents(IEnumerable<Unsafe.IComponentIdentifier<C>> addresses);
        public IOption<R> GetComponent<R>(IComponentIdentifier<C, R> address) where R : Res;
    }
}
