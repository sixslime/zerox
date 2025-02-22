#nullable enable
namespace SixShaded.FourZeroOne.Token
{
    public interface IHasAttachedComponentIdentifier<in C, out R> : IToken<R>
        where C : ICompositionType
        where R : Res
    {
        public Resolution.Unsafe.IComponentIdentifier<C> AttachedComponentIdentifier { get; }
    }
}