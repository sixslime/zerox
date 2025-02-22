namespace SixShaded.FourZeroOne.Rule.Defined;

using Unsafe;
public abstract record RuleBehavior<R> : IRule<R>
    where R : class, Res
{
    protected abstract IRuleMatcher<IToken<R>> InternalMatcher { get; }
    protected abstract Resolution.Unsafe.IBoxedMetaFunction<R> InternalDefinition { get; }
    public RuleID ID { get; } = RuleIDGenerator.Next();

    IRuleMatcher<IToken<R>> IRule<R>.MatcherUnsafe => InternalMatcher;
    Resolution.Unsafe.IBoxedMetaFunction<R> IRule<R>.DefinitionUnsafe => InternalDefinition;

    public IOption<IRuledToken<R>> TryApply(Tok token) =>
        token is IToken<R> typed && InternalMatcher.MatchesToken(token)
            ? new RuledToken<R>
            {
                AppliedRule = this,
                Proxies =
                    new Proxies.OriginalProxy<R> { Token = typed, FromRule = ID }.IsA<IProxy<Res>>().Yield()
                        .Concat(ConstructArgProxies(typed))
                        .ToArray(),
            }.AsSome()
            : new None<IRuledToken<R>>();

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
    protected Proxies.ArgProxy<RArg> CreateArgProxy<RArg>(Tok token)
        where RArg : class, Res =>
        new() { FromRule = ID, Token = token.IsA<IToken<RArg>>() };
}