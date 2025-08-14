namespace SixShaded.FourZeroOne.Core.Roggis;

public sealed record Multi<R> : Roggi.Defined.Roggi, IMulti<R>
    where R : class, Rog
{
    public required PSequence<IOption<R>> Values { get; init; }
    public override int GetHashCode() => Elements.GetHashCode();
    public override string ToString() => $"[{string.Join(", ", Elements.Map(x => x.ToString()))}]";
    public bool Equals(Multi<R>? other) => other is not null && Elements.SequenceEqual(other.Elements);
    public IEnumerable<IOption<R>> Elements => Values.Elements;
    public int Count => Values.Count;
    public override IEnumerable<IInstruction> Instructions => Elements.FilterMap(x => x.RemapAs(y => y.Instructions)).Flatten();
    public IOption<IOption<R>> At(int index) => Values.At(index);
}