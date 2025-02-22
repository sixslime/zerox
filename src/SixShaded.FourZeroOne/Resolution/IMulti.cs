namespace SixShaded.FourZeroOne.Resolution;

public interface IMulti<out R> : IHasElements<R>, IIndexReadable<int, IOption<R>>, Res where R : class, Res { }