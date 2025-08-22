namespace SixShaded.FourZeroOne.Roggi;

public sealed record DynamicRoda<R> : ILoadOverwritingRoda<R>
    where R : class, Rog
{
    private static ulong _idAssigner = 0;

    public DynamicRoda()
    {
        DynamicId = Interlocked.Increment(ref _idAssigner);
    }

    public ulong DynamicId { get; }
    public override string ToString() => $"{(typeof(R).GetHashCode() % 441).ToBase("AOEUISNTHLRCGFDSVB".ToLower(), "")}{((int)(DynamicId % 5)).ToBase("JKMWQVZX", "")}";
}