namespace SixShaded.FourZeroOne.Rule.Defined;

using Proxies;
using Core.Resolutions;

public record RuleForSignature<RVal> : RuleBehavior<RVal>, IRuleOfSignature<RVal>
    where RVal : class, Res
{
    protected override Resolution.Unsafe.IBoxedMetaFunction<RVal> InternalDefinition => Definition;
    protected override IRuleMatcher<IToken<RVal>> InternalMatcher => Matcher;
    public required MetaFunction<OriginalProxy<RVal>, RVal> Definition { get; init; }
    public required IRuleMatcher<IHasNoArgs<RVal>> Matcher { get; init; }
    protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<RVal> token) => [];
}

public record RuleForSignature<RArg1, ROut> : RuleBehavior<ROut>, IRuleOfSignature<RArg1, ROut>
    where RArg1 : class, Res
    where ROut : class, Res
{
    protected override Resolution.Unsafe.IBoxedMetaFunction<ROut> InternalDefinition => Definition;
    protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
    public required MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut> Definition { get; init; }
    public required IRuleMatcher<IHasArgs<RArg1, ROut>> Matcher { get; init; }

    protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<ROut> token)
        => [CreateArgProxy<RArg1>(token.ArgTokens[0])];
}

public record RuleForSignature<RArg1, RArg2, ROut> : RuleBehavior<ROut>, IRuleOfSignature<RArg1, RArg2, ROut>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where ROut : class, Res
{
    protected override Resolution.Unsafe.IBoxedMetaFunction<ROut> InternalDefinition => Definition;
    protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
    public required MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut> Definition { get; init; }
    public required IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>> Matcher { get; init; }

    protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<ROut> token)
        => [CreateArgProxy<RArg1>(token.ArgTokens[0]), CreateArgProxy<RArg2>(token.ArgTokens[1])];
}

public record RuleForSignature<RArg1, RArg2, RArg3, ROut> : RuleBehavior<ROut>, IRuleOfSignature<RArg1, RArg2, RArg3, ROut>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where RArg3 : class, Res
    where ROut : class, Res
{
    protected override Resolution.Unsafe.IBoxedMetaFunction<ROut> InternalDefinition => Definition;
    protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
    public required OverflowingMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut> Definition { get; init; }
    public required IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>> Matcher { get; init; }

    protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<ROut> token)
        => [CreateArgProxy<RArg1>(token.ArgTokens[0]), CreateArgProxy<RArg2>(token.ArgTokens[1]), CreateArgProxy<RArg3>(token.ArgTokens[2])];
}