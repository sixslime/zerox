namespace SixShaded.FourZeroOne.Roveggi.Defined;

using Roveggi;
using Unsafe;

// this is mega stupid.
// update from HQ, its a regular amount of stupid.
public record Roveggi<C> : Roggi.Defined.NoOp, IRoveggi<C>
    where C : IRovetu
{
    public Roveggi()
    {
        _componentMap = new();
    }

    private PMap<IRovu<C>, Rog> _componentMap { get; init; }
    public override string ToString() => $"{typeof(C).Namespace!.Split(".")[^1]}.{typeof(C).Name}:{{{string.Join(" ", ComponentsUnsafe.OrderBy(x => x.A.ToString()).Map(x => $"{x.A}={x.B}"))}}}";
    public override int GetHashCode() => _componentMap.GetHashCode();
    public virtual bool Equals(Roveggi<C>? other) => other is not null && _componentMap.Elements.Intersect(other._componentMap.Elements).Count() == _componentMap.Count;
    public IEnumerable<ITiple<IRovu, Rog>> ComponentsUnsafe => _componentMap.Elements;

    // UNBELIEVABLY stupid
    public IRoveggi<C> WithComponent<R>(IRovu<C, R> identifier, R data)
        where R : class, Rog =>
        this with
        {
            _componentMap = _componentMap.WithEntries((identifier, data).Tiple()),
        };

    public IRoveggi<C> WithComponentsUnsafe(IEnumerable<ITiple<IRovu<C>, Rog>> components) =>
        this with
        {
            _componentMap = _componentMap.WithEntries(components),
        };

    public IRoveggi<C> WithoutComponents(IEnumerable<IRovu<C>> addresses) =>
        this with
        {
            _componentMap = _componentMap.WithoutEntries(addresses),
        };

    public IOption<R> GetComponent<R>(IRovu<C, R> address)
        where R : class, Rog =>
        _componentMap.At(address).RemapAs(x => (R)x);
}