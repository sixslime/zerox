namespace SixShaded.FourZeroOne.Roggi;

public interface IMulti<out R> : IHasElements<R>, IIndexReadable<int, R>, Rog
    where R : class, Rog
{ }