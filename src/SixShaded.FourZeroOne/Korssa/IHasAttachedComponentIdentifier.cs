namespace SixShaded.FourZeroOne.Korssa;

using SixShaded.FourZeroOne.Roveggi;
using SixShaded.FourZeroOne.Roveggi.Unsafe;

public interface IHasAttachedComponentIdentifier<in C, out R> : IKorssa<R>
    where C : IRovetu
    where R : class, Rog
{
    public IRovu<C> _attachedRovu { get; }
}