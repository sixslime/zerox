namespace SixShaded.FourZeroOne.Korssa;

using Roveggi;
using Roveggi.Unsafe;

public interface IHasAttachedComponentIdentifier<in C, out R> : IKorssa<R>
    where C : IRovetu
    where R : class, Rog
{
    public IRovu<C> _attachedRovu { get; }
}