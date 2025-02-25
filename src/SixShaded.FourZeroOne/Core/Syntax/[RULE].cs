namespace SixShaded.FourZeroOne.Core.Syntax;

using Resolutions;
using Rule.Defined.Proxies;
using Rule.Defined.Matchers;

public static partial class Core
{
    public static Tokens.AddRule tAddRule<RVal>(Structure.Rule.Block<RVal> block)
        where RVal : class, Res
    {
        var vs = new DynamicAddress<MetaFunction<OriginalProxy<RVal>, RVal>>();
        var vo = new DynamicAddress<OriginalProxy<RVal>>();

        return new(new Rule.Defined.RuleForSignature<RVal> { Definition = new() { SelfIdentifier = vs, IdentifierA = vo, Token = block.Definition(vo) }, Matcher = block.Matches(new()) });
    }

    public static Tokens.AddRule tAddRule<RArg1, ROut>(Structure.Rule.Block<RArg1, ROut> block)
        where RArg1 : class, Res
        where ROut : class, Res
    {
        var vs = new DynamicAddress<MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut>>();
        var (vo, v1) = (new DynamicAddress<OriginalProxy<ROut>>(), new DynamicAddress<ArgProxy<RArg1>>());

        return new(new Rule.Defined.RuleForSignature<RArg1, ROut> { Definition = new() { SelfIdentifier = vs, IdentifierA = vo, IdentifierB = v1, Token = block.Definition(vo, v1) }, Matcher = block.Matches(new()) });
    }

    public static Tokens.AddRule tAddRule<RArg1, RArg2, ROut>(Structure.Rule.Block<RArg1, RArg2, ROut> block)
        where RArg1 : class, Res
        where RArg2 : class, Res
        where ROut : class, Res
    {
        var vs = new DynamicAddress<MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut>>();
        var (vo, v1, v2) = (new DynamicAddress<OriginalProxy<ROut>>(), new DynamicAddress<ArgProxy<RArg1>>(), new DynamicAddress<ArgProxy<RArg2>>());

        return new(new Rule.Defined.RuleForSignature<RArg1, RArg2, ROut>
        {
            Definition = new()
            {
                SelfIdentifier = vs,
                IdentifierA = vo,
                IdentifierB = v1,
                IdentifierC = v2,
                Token = block.Definition(vo, v1, v2),
            },
            Matcher = block.Matches(new()),
        });
    }

    public static Tokens.AddRule tAddRule<RArg1, RArg2, RArg3, ROut>(Structure.Rule.Block<RArg1, RArg2, RArg3, ROut> block)
        where RArg1 : class, Res
        where RArg2 : class, Res
        where RArg3 : class, Res
        where ROut : class, Res
    {
        var vs = new DynamicAddress<OverflowingMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut>>();
        var (vo, v1, v2, v3) = (new DynamicAddress<OriginalProxy<ROut>>(), new DynamicAddress<ArgProxy<RArg1>>(), new DynamicAddress<ArgProxy<RArg2>>(), new DynamicAddress<ArgProxy<RArg3>>());

        return new(new Rule.Defined.RuleForSignature<RArg1, RArg2, RArg3, ROut>
        {
            Definition = new()
            {
                SelfIdentifier = vs,
                IdentifierA = vo,
                IdentifierB = v1,
                IdentifierC = v2,
                IdentifierD = v3,
                Token = block.Definition(vo, v1, v2, v3),
            },
            Matcher = block.Matches(new()),
        });
    }
}

public static partial class TokenSyntax
{
    public static Rule.Defined.RealizeProxy<R> tRealize<R>(this IToken<IProxy<R>> proxy)
        where R : class, Res =>
        new(proxy);
}

public static class RuleMatcherSyntax
{
    public static TypeMatcher<TMatch> mIsType<TMatch>(this Structure.Rule.IMatcherBuilder _)
        where TMatch : IToken<Res> =>
        new();

    public static MacroMatcher<RVal> mIsMacro<RVal>(this Structure.Rule.MatcherBuilder<RVal> _, string package, string identifier)
        where RVal : class, Res =>
        new() { Label = new() { Package = package, Identifier = identifier } };

    public static MacroMatcher<RArg1, ROut> mIsMacro<RArg1, ROut>(this Structure.Rule.MatcherBuilder<RArg1, ROut> _, string package, string identifier)
        where RArg1 : class, Res
        where ROut : class, Res =>
        new() { Label = new() { Package = package, Identifier = identifier } };

    public static MacroMatcher<RArg1, RArg2, ROut> mIsMacro<RArg1, RArg2, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, ROut> _, string package, string identifier)
        where RArg1 : class, Res
        where RArg2 : class, Res
        where ROut : class, Res =>
        new() { Label = new() { Package = package, Identifier = identifier } };

    public static MacroMatcher<RArg1, RArg2, RArg3, ROut> mIsMacro<RArg1, RArg2, RArg3, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, RArg3, ROut> _, string package, string identifier)
        where RArg1 : class, Res
        where RArg2 : class, Res
        where RArg3 : class, Res
        where ROut : class, Res =>
        new() { Label = new() { Package = package, Identifier = identifier } };

    public static AnyMatcher<IHasNoArgs<RVal>> mAny<RVal>(this Structure.Rule.MatcherBuilder<RVal> _, List<IRuleMatcher<IHasNoArgs<RVal>>> entries)
        where RVal : class, Res =>
        new() { Entries = entries.ToPSet() };

    public static AnyMatcher<IHasArgs<RArg1, ROut>> mAny<RArg1, ROut>(this Structure.Rule.MatcherBuilder<RArg1, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, ROut>>> entries)
        where RArg1 : class, Res
        where ROut : class, Res =>
        new() { Entries = entries.ToPSet() };

    public static AnyMatcher<IHasArgs<RArg1, RArg2, ROut>> mAny<RArg1, RArg2, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>>> entries)
        where RArg1 : class, Res
        where RArg2 : class, Res
        where ROut : class, Res =>
        new() { Entries = entries.ToPSet() };

    public static AnyMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>> mAny<RArg1, RArg2, RArg3, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, RArg3, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>>> entries)
        where RArg1 : class, Res
        where RArg2 : class, Res
        where RArg3 : class, Res
        where ROut : class, Res =>
        new() { Entries = entries.ToPSet() };

    public static AllMatcher<IHasNoArgs<RVal>> mAll<RVal>(this Structure.Rule.MatcherBuilder<RVal> _, List<IRuleMatcher<IHasNoArgs<RVal>>> entries)
        where RVal : class, Res =>
        new() { Entries = entries.ToPSet() };

    public static AllMatcher<IHasArgs<RArg1, ROut>> mAll<RArg1, ROut>(this Structure.Rule.MatcherBuilder<RArg1, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, ROut>>> entries)
        where RArg1 : class, Res
        where ROut : class, Res =>
        new() { Entries = entries.ToPSet() };

    public static AllMatcher<IHasArgs<RArg1, RArg2, ROut>> mAll<RArg1, RArg2, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>>> entries)
        where RArg1 : class, Res
        where RArg2 : class, Res
        where ROut : class, Res =>
        new() { Entries = entries.ToPSet() };

    public static AllMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>> mAll<RArg1, RArg2, RArg3, ROut>(this Structure.Rule.MatcherBuilder<RArg1, RArg2, RArg3, ROut> _, List<IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>>> entries)
        where RArg1 : class, Res
        where RArg2 : class, Res
        where RArg3 : class, Res
        where ROut : class, Res =>
        new() { Entries = entries.ToPSet() };
}