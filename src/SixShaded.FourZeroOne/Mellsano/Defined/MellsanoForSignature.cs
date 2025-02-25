namespace SixShaded.FourZeroOne.Mellsano.Defined;

using Proxies;
using Core.Roggis;

public record MellsanoForSignature<RVal> : MellsanoBehavior<RVal>, IMellsanoOfSignature<RVal>
    where RVal : class, Rog
{
    protected override Roggi.Unsafe.IBoxedMetaFunction<RVal> InternalDefinition => Definition;
    protected override IUllasem<IKorssa<RVal>> InternalMatcher => Matcher;
    public required MetaFunction<OriginalProxy<RVal>, RVal> Definition { get; init; }
    public required IUllasem<IHasNoArgs<RVal>> Matcher { get; init; }
    protected override IEnumerable<IProxy<Rog>> ConstructArgProxies(IKorssa<RVal> korssa) => [];
}

public record MellsanoForSignature<RArg1, ROut> : MellsanoBehavior<ROut>, IMellsanoOfSignature<RArg1, ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    protected override Roggi.Unsafe.IBoxedMetaFunction<ROut> InternalDefinition => Definition;
    protected override IUllasem<IKorssa<ROut>> InternalMatcher => Matcher;
    public required MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut> Definition { get; init; }
    public required IUllasem<IHasArgs<RArg1, ROut>> Matcher { get; init; }

    protected override IEnumerable<IProxy<Rog>> ConstructArgProxies(IKorssa<ROut> korssa)
        => [CreateArgProxy<RArg1>(korssa.ArgKorssas[0])];
}

public record MellsanoForSignature<RArg1, RArg2, ROut> : MellsanoBehavior<ROut>, IMellsanoOfSignature<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    protected override Roggi.Unsafe.IBoxedMetaFunction<ROut> InternalDefinition => Definition;
    protected override IUllasem<IKorssa<ROut>> InternalMatcher => Matcher;
    public required MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut> Definition { get; init; }
    public required IUllasem<IHasArgs<RArg1, RArg2, ROut>> Matcher { get; init; }

    protected override IEnumerable<IProxy<Rog>> ConstructArgProxies(IKorssa<ROut> korssa)
        => [CreateArgProxy<RArg1>(korssa.ArgKorssas[0]), CreateArgProxy<RArg2>(korssa.ArgKorssas[1])];
}

public record MellsanoForSignature<RArg1, RArg2, RArg3, ROut> : MellsanoBehavior<ROut>, IMellsanoOfSignature<RArg1, RArg2, RArg3, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    protected override Roggi.Unsafe.IBoxedMetaFunction<ROut> InternalDefinition => Definition;
    protected override IUllasem<IKorssa<ROut>> InternalMatcher => Matcher;
    public required OverflowingMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut> Definition { get; init; }
    public required IUllasem<IHasArgs<RArg1, RArg2, RArg3, ROut>> Matcher { get; init; }

    protected override IEnumerable<IProxy<Rog>> ConstructArgProxies(IKorssa<ROut> korssa)
        => [CreateArgProxy<RArg1>(korssa.ArgKorssas[0]), CreateArgProxy<RArg2>(korssa.ArgKorssas[1]), CreateArgProxy<RArg3>(korssa.ArgKorssas[2])];
}