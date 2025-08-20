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
            if (!dict.TryAdd(v, i.Yield().ToPSequence()))
                dict[v] = dict[v].WithEntries(i);
        }
        _indexMap = new PMap<R, IPSequence<int>>(dict);
    }
    private Multi(IPSequence<IOption<R>> sequence, IPMap<R, IPSequence<int>> indexMap)
    {
        _sequence = sequence;
        _indexMap = indexMap;
    }

    public int Count => _sequence.Count;
    public IEnumerable<IOption<R>> Elements => _sequence.Elements;
    IPSequence<IOption<R>> IEfficientMulti<R>.Values => _sequence;
    IPMap<R, IPSequence<int>> IEfficientMulti<R>.IndexMap => _indexMap;

    IEfficientMulti<R> IEfficientMulti<R>.Distinct()
    {
        var sequence =
            _sequence.Elements
                .Enumerate()
                .Where(x => x.value.Check(out var v) && _indexMap.At(v).Unwrap().At(0).Unwrap() == x.index)
                .Map(x => x.value)
                .ToPSequence();
        var indexMap =
            _indexMap.WithEntries(
            _indexMap.Elements
                .Where(x => x.B.Count > 1)
                .Map(x => (x.A, x.B.At(0).Unwrap().Yield().ToPSequence()).Tiple()));
        return new Multi<R>(sequence, indexMap);
    }

    IEfficientMulti<R> IEfficientMulti<R>.Concat(IEfficientMulti<R> other)
    {
        var sequence = _sequence.WithEntries(other.Values.Elements);
        var indexMap =
            _indexMap.WithEntries(
            other.IndexMap.Elements
                .Map(
                pair =>
                    (_indexMap.At(pair.A).Check(out var existing)
                        ? existing.WithEntries(pair.B.Elements.Map(i => i += _sequence.Count))
                        : pair.B)
                    .ExprAs(val => (pair.A, val).Tiple())));
        return new Multi<R>(sequence, indexMap);
    }

    IEfficientMulti<R> IEfficientMulti<R>.Slice(Range range) => new Multi<R>(_sequence.Elements.WhereIndex(x => x >= range.Start.Value && x < range.End.Value));

    IEfficientMulti<R> IEfficientMulti<R>.Union(IEfficientMulti<R> other) => new Multi<R>(_sequence.Elements.Filtered().Union(other.Values.Elements.Filtered()).Map(x => x.AsSome()));

    IEfficientMulti<R> IEfficientMulti<R>.Intersect(IEfficientMulti<R> other) => new Multi<R>(_sequence.Elements.Filtered().Intersect(other.Values.Elements.Filtered()).Map(x => x.AsSome()));

    IEfficientMulti<R> IEfficientMulti<R>.Except(IEfficientMulti<R> other) => new Multi<R>(_sequence.Elements.Filtered().Except(other.Values.Elements.Filtered()).Map(x => x.AsSome()));
    Multi<R> IEfficientMulti<R>.ToMulti() => this;
    IOption<IOption<R>> IIndexReadable<int, IOption<R>>.At(int index) => _sequence.At(index);
    public override int GetHashCode() => _sequence.GetHashCode();
    public override string ToString() => $"[{string.Join(", ", _sequence.Elements.Map(x => x.Check(out var val) ? val.ToString() : "\u2205"))}]";
    public bool Equals(Multi<R>? other) => other is not null && _sequence.Elements.SequenceEqual(other._sequence.Elements);
    public override IEnumerable<IInstruction> Instructions => _sequence.Elements.FilterMap(x => x.RemapAs(y => y.Instructions)).Flatten();

}