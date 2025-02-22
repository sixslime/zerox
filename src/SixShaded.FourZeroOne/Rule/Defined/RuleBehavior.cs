#nullable enable
namespace SixShaded.FourZeroOne.Rule.Defined
{
    public abstract record RuleBehavior<R> : IRule<R>
        where R : class, Res
    {
        public RuleID ID { get; } = RuleIDGenerator.Next();
        protected abstract IRuleMatcher<IToken<R>> InternalMatcher { get; }
        protected abstract IBoxedMetaFunction<R> InternalDefinition { get; }

        /// <summary>
        /// Kinda stupid that this has to exist, but the alternative is dynamic instantiation via reflection in TryApply(). <br></br>
        /// <i>Or I guess fully type-unsafe proxies but who wants that.</i>
        /// </summary>
        protected abstract IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<R> token);
        /// <summary>
        /// <paramref name="token"/> is casted to token of <typeparamref name="RArg"/>.
        /// </summary>
        /// <typeparam name="RArg"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        protected ArgProxy<RArg> CreateArgProxy<RArg>(Token token)
            where RArg : class, Res
        {
            return new() { FromRule = ID, Token = token.IsA<IToken<RArg>>() };
        }
        IRuleMatcher<IToken<R>> IRule<R>.MatcherUnsafe => InternalMatcher;
        IBoxedMetaFunction<R> IRule<R>.DefinitionUnsafe => InternalDefinition;

        public IOption<IRuledToken<R>> TryApply(Token token)
        {
            return (token is IToken<R> typed && InternalMatcher.MatchesToken(token))
                ? new RuledToken<R>
                {
                    AppliedRule = this,
                    Proxies =
                        new OriginalProxy<R>() { Token = typed, FromRule = ID }.IsA<IProxy<Res>>().Yield()
                        .Concat(ConstructArgProxies(typed))
                        .ToArray()
                }.AsSome()
                : new None<IRuledToken<R>>();
        }
    }
}
