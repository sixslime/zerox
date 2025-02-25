namespace SixShaded.FourZeroOne.Mellsano.Defined;

using Unsafe;

public abstract record MellsanoBehavior<R> : IMellsano<R>
    where R : class, Rog
{
    protected abstract IUllasem<IKorssa<R>> InternalMatcher { get; }
    protected abstract Roggi.Unsafe.IBoxedMetaFunction<R> InternalDefinition { get; }
    public MellsanoID ID { get; } = MellsanoIDGenerator.Next();

    IUllasem<IKorssa<R>> IMellsano<R>.MatcherUnsafe => InternalMatcher;
    Roggi.Unsafe.IBoxedMetaFunction<R> IMellsano<R>.DefinitionUnsafe => InternalDefinition;

    public IOption<IKorssaOfMellsano<R>> TryApply(Kor korssa) =>
        korssa is IKorssa<R> typed && InternalMatcher.MatchesKorssa(korssa)
            ? new KorssaOfMellsano<R>
            {
                AppliedMellsano = this,
                Proxies =
                    new Proxies.OriginalProxy<R> { Korssa = typed, FromMellsano = ID }.IsA<IProxy<Rog>>().Yield()
                        .Concat(ConstructArgProxies(typed))
                        .ToArray(),
            }.AsSome()
            : new None<IKorssaOfMellsano<R>>();

    /// <summary>
    ///     Kinda stupid that this has to exist, but the alternative is dynamic instantiation via reflection in TryApply().
    ///     <br></br>
    ///     <i>Or I guess fully type-unsafe proxies but who wants that.</i>
    /// </summary>
    protected abstract IEnumerable<IProxy<Rog>> ConstructArgProxies(IKorssa<R> korssa);

    /// <summary>
    ///     <paramref name="korssa" /> is casted to korssa of <typeparamref name="RArg" />.
    /// </summary>
    /// <typeparam name="RArg"></typeparam>
    /// <param name="korssa"></param>
    /// <returns></returns>
    protected Proxies.ArgProxy<RArg> CreateArgProxy<RArg>(Kor korssa)
        where RArg : class, Rog =>
        new() { FromMellsano = ID, Korssa = korssa.IsA<IKorssa<RArg>>() };
}