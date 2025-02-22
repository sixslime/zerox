#nullable enable
namespace SixShaded.FourZeroOne.Rule.Defined
{
    using Core.Resolutions.Boxed;
    using Proxies;
    public record RuleForValue<RVal> : RuleBehavior<RVal>, IRuleOfValue<RVal>
        where RVal : class, Res
    {
        public required MetaFunction<OriginalProxy<RVal>, RVal> Definition { get; init; }
        public required IRuleMatcher<IHasNoArgs<RVal>> Matcher { get; init; }
        protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<RVal> token) => [];
        protected override Resolution.Unsafe.IBoxedMetaFunction<RVal> InternalDefinition => Definition;
        protected override IRuleMatcher<IToken<RVal>> InternalMatcher => Matcher;
    }
    public record RuleForFunction<RArg1, ROut> : RuleBehavior<ROut>, IRuleOfFunction<RArg1, ROut>
        where RArg1 : class, Res
        where ROut : class, Res
    {
        public required MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut> Definition { get; init; }
        public required IRuleMatcher<IHasArgs<RArg1, ROut>> Matcher { get; init; }
        protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<ROut> token)
            => [CreateArgProxy<RArg1>(token.ArgTokens[0])];
        protected override Resolution.Unsafe.IBoxedMetaFunction<ROut> InternalDefinition => Definition;
        protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
    }
    public record RuleForFunction<RArg1, RArg2, ROut> : RuleBehavior<ROut>, IRuleOfFunction<RArg1, RArg2, ROut>
        where RArg1 : class, Res
        where RArg2 : class, Res
        where ROut : class, Res
    {
        public required MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut> Definition { get; init; }
        public required IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>> Matcher { get; init; }
        protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<ROut> token)
            => [CreateArgProxy<RArg1>(token.ArgTokens[0]), CreateArgProxy<RArg2>(token.ArgTokens[1])];
        protected override Resolution.Unsafe.IBoxedMetaFunction<ROut> InternalDefinition => Definition;
        protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
    }
    public record RuleForFunction<RArg1, RArg2, RArg3, ROut> : RuleBehavior<ROut>, IRuleOfFunction<RArg1, RArg2, RArg3, ROut>
        where RArg1 : class, Res
        where RArg2 : class, Res
        where RArg3 : class, Res
        where ROut : class, Res
    {
        public required OverflowingMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut> Definition { get; init; }
        public required IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>> Matcher { get; init; }
        protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<ROut> token)
            => [CreateArgProxy<RArg1>(token.ArgTokens[0]), CreateArgProxy<RArg2>(token.ArgTokens[1]), CreateArgProxy<RArg3>(token.ArgTokens[2])];
        protected override Resolution.Unsafe.IBoxedMetaFunction<ROut> InternalDefinition => Definition;
        protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
    }
}
