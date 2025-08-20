namespace SixShaded.FourZeroOne.Roggi;

public interface IMulti<out R> : IHasElements<IOption<R>>, IIndexReadable<int, IOption<R>>, Rog
    where R : class, Rog
{
    /// <summary>
    /// Must be in same order of Elements.
    /// </summary>
    public IEnumerable<R> DistinctElements { get; }
}