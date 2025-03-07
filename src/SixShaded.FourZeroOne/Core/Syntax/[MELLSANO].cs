namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Mellsano.Defined.Proxies;
using Mellsano.Defined.Ullasems;

public static partial class Core
{
    public static Korssas.AddMellsano kAddMellsano<RVal>(Structure.Mellsano.Block<RVal> block)
        where RVal : class, Rog
    {
        return new(new Mellsano.Defined.Mellsano<RVal>
        {
            Definition = new((_, orig) => block.Definition(orig))
            {
                Captures = block.DefinitionCaptures.ToArray()
            },
            Matcher = block.Matches(new()),
        });
    }
    public static Korssas.AddMellsano kAddMellsano<RArg1, ROut>(Structure.Mellsano.Block<RArg1, ROut> block)
        where RArg1 : class, Rog
        where ROut : class, Rog
    {
        return new(new Mellsano.Defined.Mellsano<RArg1, ROut>
        {
            Definition = new((_, orig, argA) => block.Definition(orig, argA))
            {
                Captures = block.DefinitionCaptures.ToArray()
            },
            Matcher = block.Matches(new()),
        });
    }
    public static Korssas.AddMellsano kAddMellsano<RArg1, RArg2, ROut>(Structure.Mellsano.Block<RArg1, RArg2, ROut> block)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where ROut : class, Rog
    {
        return new(new Mellsano.Defined.Mellsano<RArg1, RArg2, ROut>
        {
            Definition = new((_, orig, argA, argB) => block.Definition(orig, argA, argB))
            {
                Captures = block.DefinitionCaptures.ToArray()
            },
            Matcher = block.Matches(new()),
        });
    }
    public static Korssas.AddMellsano kAddMellsano<RArg1, RArg2, RArg3, ROut>(Structure.Mellsano.Block<RArg1, RArg2, RArg3, ROut> block)
        where RArg1 : class, Rog
        where RArg2 : class, Rog
        where RArg3 : class, Rog
        where ROut : class, Rog
    {
        return new(new Mellsano.Defined.Mellsano<RArg1, RArg2, RArg3, ROut>
        {
            Definition = new((_, orig, argA, argB, argC) => block.Definition(orig, argA, argB, argC))
            {
                Captures = block.DefinitionCaptures.ToArray()
            },
            Matcher = block.Matches(new()),
        });
    }
}

public static partial class KorssaSyntax
{
    public static Mellsano.Defined.RealizeProxy<R> kRealize<R>(this IKorssa<IProxy<R>> proxy)
        where R : class, Rog =>
        new(proxy);
}

public static class MellsanoMatcherSyntax
{
    public static KorssaTypeUllasem<TMatch> mIsType<TMatch>(this Structure.Mellsano.IMatcherBuilder _)
        where TMatch : Kor =>
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