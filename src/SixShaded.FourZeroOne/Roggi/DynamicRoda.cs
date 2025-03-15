namespace SixShaded.FourZeroOne.Roggi;

public sealed record DynamicRoda<R> : IRoda<R>
    where R : class, Rog
{
    private static int _idAssigner;

    public DynamicRoda()
    {
        DynamicId = _idAssigner++;
    }

    public int DynamicId { get; }
    public override string ToString() => $"{(DynamicId % 5).ToBase("AOEUI", "")}{(typeof(R).GetHashCode() % 441).ToBase("DHTNSYFPGCRLVWMBXKJQZ".ToLower(), "")}";
}