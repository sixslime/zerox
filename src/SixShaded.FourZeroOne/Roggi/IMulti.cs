namespace SixShaded.FourZeroOne.Roggi;

public interface IMulti<out R> : IHasElements<IOption<R>>, IIndexReadable<int, IOption<R>>, Rog
    where R : class, Rog
{ }