namespace SixShaded.FourZeroOne.Core.Roggis;

using Roggi.Unsafe;
public sealed record Multi<R> : Roggi.Defined.Roggi, IEfficientMulti<R>
    where R : class, Rog
{
    private readonly IPMap<R, IPSequence<int>> _indexMap = null!;
    private readonly IPSequence<IOption<R>> _sequence = null!;

    public Multi(IEnumerable<IOption<R>> values)
    {
        _sequence = values.ToPSequence();
        var dict = new Dictionary<R, IPSequence<int>>();
        foreach (var (i, opt) in _sequence.Elements.Enumerate())
        {
            if (opt.CheckNone(out var v))
                continue;
            if (!dict.TryAdd(v, new PSequence<int>([i])))
                dict[v] = dict[v].WithEntries(i);
        }
        _indexMap = new PMap<R, IPSequence<int>>(dict);
    }
    private Multi(IPSequence<IOption<R>> sequence, IPMap<R, IPSequence<int>> indexMap) : this()
    {
        _sequence = sequence;
        _indexMap = indexMap;
    }

    IPSequence<IOption<R>> IEfficientMulti<R>.Values => _sequence;
    IPMap<R, IPSequence<int>> IEfficientMulti<R>.IndexMap => _indexMap;

    IEfficientMulti<R> IEfficientMulti<R>.Distinct() => throw new NotImplementedException();

    IEfficientMulti<R> IEfficientMulti<R>.Concat(IEfficientMulti<R> other)
    {
        var sequence = _sequence.WithEntries(other.Values.Elements);
        var indexMap =
            other.Values.Elements
                .Enumerate()
                .AccumulateInto(
                _indexMap,
                (map, ipair) =>
                    ipair.value.RemapAs(
                        element =>
                        {
                            int nIndex = ipair.index + _sequence.Count;
                            return map.WithEntryOrUpdate(element, () => new PSequence<int>([nIndex]), list => list.WithEntries(nIndex));
                        })
                        .Or(map));
        return new Multi<R>(sequence, indexMap);
    }

    IEfficientMulti<R> IEfficientMulti<R>.Slice(Range range) => throw new NotImplementedException();

    IEfficientMulti<R> IEfficientMulti<R>.Union(IEfficientMulti<R> other) => throw new NotImplementedException();

    IEfficientMulti<R> IEfficientMulti<R>.Intersect(IEfficientMulti<R> other) => throw new NotImplementedException();

    IEfficientMulti<R> IEfficientMulti<R>.Inversect(IEfficientMulti<R> other) => throw new NotImplementedException();

    IOption<IOption<R>> IIndexReadable<int, IOption<R>>.At(int index) => throw new NotImplementedException();
    public override int GetHashCode() => Values.GetHashCode();
    public override string ToString() => $"[{string.Join(", ", _sequence.Elements.Map(x => x.Check(out var val) ? val.ToString() : "\u2205"))}]";
    public bool Equals(Multi<R>? other) => other is not null && _sequence.Elements.SequenceEqual(other._sequence.Elements);
    public override IEnumerable<IInstruction> Instructions => _sequence.Elements.FilterMap(x => x.RemapAs(y => y.Instructions)).Flatten();

    int IHasElements<IOption<R>>.Count => _sequence.Count;

    IEnumerable<IOption<R>> IHasElements<IOption<R>>.Elements => _sequence.Elements;

}