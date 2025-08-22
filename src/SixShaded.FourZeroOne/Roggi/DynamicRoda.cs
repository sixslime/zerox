namespace SixShaded.FourZeroOne.Roggi;

public sealed record DynamicRoda<R> : ILoadOverridingRoda<R>
    where R : class, Rog
{
    private static ulong _idAssigner = 0;

    public DynamicRoda()
    {
        DynamicId = Interlocked.Increment(ref _idAssigner);
    }

    public ulong DynamicId { get; }

    public override string ToString() =>
        typeof(R).ExprAs(type =>
            $"{(type.Name.Length > 3 ? type.Name[0..3] : type.Name).ToLower()}" +
            $"{type.GenericTypeArguments.Map(x => (x.Name.Length > 2 ? x.Name[1].ToString().ToUpper() + x.Name[2].ToString().ToLower() : x.Name)).Accumulate((s, e) => s + e).Or("")}" +
            $"{((int)DynamicId).ToBase("ABCDEFGH").PadLeft(2, 'A')[^2..]}");
}