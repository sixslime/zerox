namespace SixShaded.FourZeroOne.Core.Syntax.Structure.Mellsano;

using FourZeroOne.Mellsano.Defined.Proxies;

public sealed record Block<RVal>
    where RVal : class, Rog
{
    public required Func<MatcherBuilder<RVal>, IUllasem<IHasNoArgs<RVal>>> Matches { get; init; }
    public required Func<DynamicRoda<OriginalProxy<RVal>>, IKorssa<RVal>> Definition { get; init; }
    public IEnumerable<Addr> DefinitionCaptures { get; init; } = [];
}

public sealed record Block<RArg1, ROut>
    where RArg1 : class, Rog
    where ROut : class, Rog
{
    public required Func<MatcherBuilder<RArg1, ROut>, IUllasem<IHasArgs<RArg1, ROut>>> Matches { get; init; }
    public required Func<DynamicRoda<OriginalProxy<ROut>>, DynamicRoda<ArgProxy<RArg1>>, IKorssa<ROut>> Definition { get; init; }
    public IEnumerable<Addr> DefinitionCaptures { get; init; } = [];
}

public sealed record Block<RArg1, RArg2, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{
    public required Func<MatcherBuilder<RArg1, RArg2, ROut>, IUllasem<IHasArgs<RArg1, RArg2, ROut>>> Matches { get; init; }
    public required Func<DynamicRoda<OriginalProxy<ROut>>, DynamicRoda<ArgProxy<RArg1>>, DynamicRoda<ArgProxy<RArg2>>, IKorssa<ROut>> Definition { get; init; }
    public IEnumerable<Addr> DefinitionCaptures { get; init; } = [];
}

public sealed record Block<RArg1, RArg2, RArg3, ROut>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{
    public required Func<MatcherBuilder<RArg1, RArg2, RArg3, ROut>, IUllasem<IHasArgs<RArg1, RArg2, RArg3, ROut>>> Matches { get; init; }
    public required Func<DynamicRoda<OriginalProxy<ROut>>, DynamicRoda<ArgProxy<RArg1>>, DynamicRoda<ArgProxy<RArg2>>, DynamicRoda<ArgProxy<RArg3>>, IKorssa<ROut>> Definition { get; init; }
    public IEnumerable<Addr> DefinitionCaptures { get; init; } = [];
}