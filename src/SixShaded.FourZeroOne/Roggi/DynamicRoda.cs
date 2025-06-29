namespace SixShaded.FourZeroOne.Roggi;

public sealed record DynamicRoda<R> : IRoda<R>
    where R : class, Rog
{
    private static ulong _idAssigner = 0;

    public DynamicRoda()
    {
        DynamicId = Interlocked.Increment(ref _idAssigner);
    }

    public ulong DynamicId { get; }
    public override string ToString() => $"{((int)(DynamicId % 5)).ToBase("JKMWQVZX", "")}{(typeof(R).GetHashCode() % 441).ToBase("AOEUISNTHLRCGFDSVB".ToLower(), "")}";
}