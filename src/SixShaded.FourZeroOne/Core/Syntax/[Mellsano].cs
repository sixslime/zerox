namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Mellsano.Defined.Proxies;
using Mellsano.Defined.Ullasems;

public static partial class Core
{
    public static Korssas.AddMellsano tAddMellsano<RVal>(Structure.Mellsano.Block<RVal> block)
        where RVal : class, Rog
    {
        var vs = new DynamicAddress<MetaFunction<OriginalProxy<RVal>, RVal>>();
        var vo = new DynamicAddress<OriginalProxy<RVal>>();

        return new(new Mellsano.Defined.MellsanoForSignature<RVal> { Definition = new() { SelfIdentifier = vs, IdentifierA = vo, Korssa = block.Definition(vo) }, Matcher = block.Matches(new()) });
    }

    public static Korssas.AddMellsano tAddMellsano<RArg1, ROut>(Structure.Mellsano.Block<RArg1, ROut> block)
        where RArg1 : class, Rog
        where ROut : class, Rog
    {
        var vs = new DynamicAddress<MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut>>();
        var (vo, v1) = (new DynamicAddress<OriginalProxy<ROut>>(), new DynamicAddress<ArgProxy<RArg1>>());

        return new(new Mellsano.Defined.MellsanoForSignature<RArg1, ROut> { Definition = new() { SelfIdentifier = vs, IdentifierA = vo, IdentifierB = v1, Korssa = block.Definition(vo, v1) }, Matcher = block.Matches(new()) });
    }

    public static Korssas.AddMellsano tAddMellsano<RArg1, RArg2, ROut>(Structure.Mellsano.Block<RArg1, RArg2, ROut> block)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where ROut : class, Rog
    {
        var vs = new DynamicAddress<MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut>>();
        var (vo, v1, v2) = (new DynamicAddress<OriginalProxy<ROut>>(), new DynamicAddress<ArgProxy<RArg1>>(), new DynamicAddress<ArgProxy<RArg2>>());

        return new(new Mellsano.Defined.MellsanoForSignature<RArg1, RArg2, ROut>
        {
            Definition = new()
            {
                SelfIdentifier = vs,
                IdentifierA = vo,
                IdentifierB = v1,
                IdentifierC = v2,
                Korssa = block.Definition(vo, v1, v2),
            },
            Matcher = block.Matches(new()),
        });
    }

    public static Korssas.AddMellsano tAddMellsano<RArg1, RArg2, RArg3, ROut>(Structure.Mellsano.Block<RArg1, RArg2, RArg3, ROut> block)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where RArg3 : class, Rog
        where ROut : class, Rog
    {
        var vs = new DynamicAddress<OverflowingMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut>>();
        var (vo, v1, v2, v3) = (new DynamicAddress<OriginalProxy<ROut>>(), new DynamicAddress<ArgProxy<RArg1>>(), new DynamicAddress<ArgProxy<RArg2>>(), new DynamicAddress<ArgProxy<RArg3>>());

        return new(new Mellsano.Defined.MellsanoForSignature<RArg1, RArg2, RArg3, ROut>
        {
            Definition = new()
            {
                SelfIdentifier = vs,
                IdentifierA = vo,
                IdentifierB = v1,
                IdentifierC = v2,
                IdentifierD = v3,
                Korssa = block.Definition(vo, v1, v2, v3),
            },
            Matcher = block.Matches(new()),
        });
    }
}

public static partial class KorssaSyntax
{
    public static Mellsano.Defined.RealizeProxy<R> tRealize<R>(this IKorssa<IProxy<R>> proxy)
        where R : class, Rog =>
        new(proxy);
}

public static class MellsanoMatcherSyntax
{
    public static KorssaTypeUllasem<TMatch> mIsType<TMatch>(this Structure.Mellsano.IMatcherBuilder _)
        where TMatch : IKorssa<Rog> =>
        new();

    public static KorvessaUllasem<RVal> mIsKorvessa<RVal>(this Structure.Mellsano.MatcherBuilder<RVal> _, Axodu axoiSource, string identifier)
        where RVal : class, Rog =>
        new() { Dusem = new(axoiSource, identifier) };

    public static KorvessaUllasem<RArg1, ROut> mIsKorvessa<RArg1, ROut>(this Structure.Mellsano.MatcherBuilder<RArg1, ROut> _, Axodu axoiSource, string identifier)
        where RArg1 : class, Rog
        where ROut : class, Rog =>
        new() { Dusem = new(axoiSource, identifier) };

    public static KorvessaUllasem<RArg1, RArg2, ROut> mIsKorvessa<RArg1, RArg2, ROut>(this Structure.Mellsano.MatcherBuilder<RArg1, RArg2, ROut> _, Axodu axoiSource, string identifier)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where ROut : class, Rog =>
        new() { Dusem = new(axoiSource, identifier) };

    public static KorvessaUllasem<RArg1, RArg2, RArg3, ROut> mIsKorvessa<RArg1, RArg2, RArg3, ROut>(this Structure.Mellsano.MatcherBuilder<RArg1, RArg2, RArg3, ROut> _, Axodu axoiSource, string identifier)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where RArg3 : class, Rog
        where ROut : class, Rog =>
        new() { Dusem = new(axoiSource, identifier) };

    public static AnyUllasem<IHasNoArgs<RVal>> mAny<RVal>(this Structure.Mellsano.MatcherBuilder<RVal> _, List<IUllasem<IHasNoArgs<RVal>>> entries)
        where RVal : class, Rog =>
        new() { Entries = entries.ToPSet() };

    public static AnyUllasem<IHasArgs<RArg1, ROut>> mAny<RArg1, ROut>(this Structure.Mellsano.MatcherBuilder<RArg1, ROut> _, List<IUllasem<IHasArgs<RArg1, ROut>>> entries)
        where RArg1 : class, Rog
        where ROut : class, Rog =>
        new() { Entries = entries.ToPSet() };

    public static AnyUllasem<IHasArgs<RArg1, RArg2, ROut>> mAny<RArg1, RArg2, ROut>(this Structure.Mellsano.MatcherBuilder<RArg1, RArg2, ROut> _, List<IUllasem<IHasArgs<RArg1, RArg2, ROut>>> entries)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where ROut : class, Rog =>
        new() { Entries = entries.ToPSet() };

    public static AnyUllasem<IHasArgs<RArg1, RArg2, RArg3, ROut>> mAny<RArg1, RArg2, RArg3, ROut>(this Structure.Mellsano.MatcherBuilder<RArg1, RArg2, RArg3, ROut> _, List<IUllasem<IHasArgs<RArg1, RArg2, RArg3, ROut>>> entries)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where RArg3 : class, Rog
        where ROut : class, Rog =>
        new() { Entries = entries.ToPSet() };

    public static AllUllasem<IHasNoArgs<RVal>> mAll<RVal>(this Structure.Mellsano.MatcherBuilder<RVal> _, List<IUllasem<IHasNoArgs<RVal>>> entries)
        where RVal : class, Rog =>
        new() { Entries = entries.ToPSet() };

    public static AllUllasem<IHasArgs<RArg1, ROut>> mAll<RArg1, ROut>(this Structure.Mellsano.MatcherBuilder<RArg1, ROut> _, List<IUllasem<IHasArgs<RArg1, ROut>>> entries)
        where RArg1 : class, Rog
        where ROut : class, Rog =>
        new() { Entries = entries.ToPSet() };

    public static AllUllasem<IHasArgs<RArg1, RArg2, ROut>> mAll<RArg1, RArg2, ROut>(this Structure.Mellsano.MatcherBuilder<RArg1, RArg2, ROut> _, List<IUllasem<IHasArgs<RArg1, RArg2, ROut>>> entries)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where ROut : class, Rog =>
        new() { Entries = entries.ToPSet() };

    public static AllUllasem<IHasArgs<RArg1, RArg2, RArg3, ROut>> mAll<RArg1, RArg2, RArg3, ROut>(this Structure.Mellsano.MatcherBuilder<RArg1, RArg2, RArg3, ROut> _, List<IUllasem<IHasArgs<RArg1, RArg2, RArg3, ROut>>> entries)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where RArg3 : class, Rog
        where ROut : class, Rog =>
        new() { Entries = entries.ToPSet() };
}