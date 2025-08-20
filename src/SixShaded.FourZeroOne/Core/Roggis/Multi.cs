namespace SixShaded.FourZeroOne.Core.Roggis;

public sealed record Multi<R> : Roggi.Defined.Roggi, IMulti<R>
    where R : class, Rog
{
    private readonly PMap<R, List<int>> _indexMap = null!;
    private readonly IPSequence<IOption<R>> _sequence = null!;
    public required IPSequence<IOption<R>> Values
    {
        get => _sequence;
        init
        {
            _sequence = value;
            var dict = new Dictionary<R, List<int>>();
            foreach (var (i, opt) in value.Elements.Enumerate())
            {
                if (opt.CheckNone(out var v))
                    continue;
                if (!dict.TryAdd(v, [i]))
                    dict[v].Add(i);
            }
            _indexMap = new(dict);
        }
    }

    private IEnumerable<R> DistinctIterator()
    {
        foreach (var opt in _sequence.Elements)
        {
            if (opt.CheckNone(out var v))
                continue;

        }
    }
    public override int GetHashCode() => Values.GetHashCode();
    public override string ToString() => $"[{string.Join(", ", Elements.Map(x => x.Check(out var val) ? val.ToString() : "\u2205"))}]";
    public bool Equals(Multi<R>? other) => other is not null && Elements.SequenceEqual(other.Elements);
    public IEnumerable<IOption<R>> Elements => Values.Elements;
    public int Count => Values.Count;
    public override IEnumerable<IInstruction> Instructions => Elements.FilterMap(x => x.RemapAs(y => y.Instructions)).Flatten();
    public IOption<IOption<R>> At(int index) => Values.At(index);
}