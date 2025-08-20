namespace SixShaded.FourZeroOne.Roggi.Unsafe;

// TODO/DEV:
// perhaps mark as "IInternalType" so clients know not to display.
public interface IEfficientMulti<R> : IMulti<R>
where R : class, Rog
{
    public IPSequence<IOption<R>> Values { get; }
    public IPMap<R, IPSequence<int>> IndexMap { get; }
    public IEfficientMulti<R> Distinct();
    public IEfficientMulti<R> Concat(IEfficientMulti<R> other);
    public IEfficientMulti<R> Slice(Range range);
    public IEfficientMulti<R> Union(IEfficientMulti<R> other);
    public IEfficientMulti<R> Intersect(IEfficientMulti<R> other);
    public IEfficientMulti<R> Except(IEfficientMulti<R> other);
}