namespace SixShaded.FourZeroOne.Core.Roggis;

public sealed record Multi<R> : Roggi.Defined.Roggi, IMulti<R>
    where R : class, Rog
{
    public required PSequence<R> Values { get; init; }

    public Updater<PSequence<R>> dValues
    {
        init => Values = value(Values);
    }

    public override int GetHashCode() => Elements.GetHashCode();
    public override string ToString() => $"[{string.Join(", ", Elements.Map(x => x.ToString()))}]";
    public bool Equals(Multi<R>? other) => other is not null && Elements.SequenceEqual(other.Elements);
    public IEnumerable<R> Elements => Values.Elements;
    public int Count => Values.Count;
    public override IEnumerable<IInstruction> Instructions => Elements.Map(x => x.Instructions).Flatten();

    public IOption<R> At(int index)
    {
        try
        {
            return Values.At(index).AsSome();
        }
        catch
        {
            return new None<R>();
        }
    }
}