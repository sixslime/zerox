namespace SixShaded.FourZeroOne.Resolution;

public sealed record DynamicAddress<R> : IMemoryAddress<R> where R : class, Res
{
    private static int _idAssigner;

    public DynamicAddress()
    {
        DynamicId = _idAssigner++;
    }

    public int DynamicId { get; }
    public override string ToString() => $"{(DynamicId % 5).ToBase("AOEUI", "")}{(typeof(R).GetHashCode() % 441).ToBase("DHTNSYFPGCRLVWMBXKJQZ".ToLower(), "")}";
}