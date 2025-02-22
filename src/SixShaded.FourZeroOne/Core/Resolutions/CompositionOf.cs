namespace SixShaded.FourZeroOne.Core.Resolutions;

// the 'new()' constraint is mega stupid.
// this is mega stupid.
// update from HQ, its a regular amount of stupid.
public record CompositionOf<C> : Resolution.Defined.NoOp, ICompositionOf<C> where C : ICompositionType, new()
{
    public CompositionOf()
    {
        _componentMap = new();
    }

    private PMap<Resolution.Unsafe.IComponentIdentifier<C>, Res> _componentMap { get; init; }

    public IEnumerable<ITiple<Resolution.Unsafe.IComponentIdentifier, Res>> ComponentsUnsafe => _componentMap.Elements;

    // UNBELIEVABLY stupid
    public ICompositionOf<C> WithComponent<R>(IComponentIdentifier<C, R> identifier, R data) where R : class, Res => this with { _componentMap = _componentMap.WithEntries((identifier.IsA<Resolution.Unsafe.IComponentIdentifier<C>>(), data.IsA<Res>()).Tiple()) };

    public ICompositionOf<C> WithComponentsUnsafe(IEnumerable<ITiple<Resolution.Unsafe.IComponentIdentifier<C>, Res>> components) => this with { _componentMap = _componentMap.WithEntries(components) };

    public ICompositionOf<C> WithoutComponents(IEnumerable<Resolution.Unsafe.IComponentIdentifier<C>> addresses) => this with { _componentMap = _componentMap.WithoutEntries(addresses) };

    public IOption<R> GetComponent<R>(IComponentIdentifier<C, R> address) where R : class, Res => _componentMap.At(address).RemapAs(x => (R)x);

    public virtual bool Equals(CompositionOf<C>? other) => other is not null && ComponentsUnsafe.SequenceEqual(other.ComponentsUnsafe);

    public override string ToString() => $"{typeof(C).Namespace!.Split(".")[^1]}.{typeof(C).Name}:{{{string.Join(" ", ComponentsUnsafe.OrderBy(x => x.A.ToString()).Map(x => $"{x.A}={x.B}"))}}}";

    public override int GetHashCode() => ComponentsUnsafe.GetHashCode();
}