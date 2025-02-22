namespace SixShaded.FourZeroOne.Core.Syntax.Structure.Rule;

using FourZeroOne.Rule.Defined.Proxies;
public sealed record Block<RVal>
    where RVal : class, Res
{
    public required Func<MatcherBuilder<RVal>, IRuleMatcher<IHasNoArgs<RVal>>> Matches { get; init; }
    public required Func<DynamicAddress<OriginalProxy<RVal>>, IToken<RVal>> Definition { get; init; }
}
public sealed record Block<RArg1, ROut>
    where RArg1 : class, Res
    where ROut : class, Res
{
    public required Func<MatcherBuilder<RArg1, ROut>, IRuleMatcher<IHasArgs<RArg1, ROut>>> Matches { get; init; }
    public required Func<DynamicAddress<OriginalProxy<ROut>>, DynamicAddress<ArgProxy<RArg1>>, IToken<ROut>> Definition { get; init; }
}
public sealed record Block<RArg1, RArg2, ROut>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where ROut : class, Res
{
    public required Func<MatcherBuilder<RArg1, RArg2, ROut>, IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>>> Matches { get; init; }
    public required Func<DynamicAddress<OriginalProxy<ROut>>, DynamicAddress<ArgProxy<RArg1>>, DynamicAddress<ArgProxy<RArg2>>, IToken<ROut>> Definition { get; init; }
}
public sealed record Block<RArg1, RArg2, RArg3, ROut>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where RArg3 : class, Res
    where ROut : class, Res
{
    public required Func<MatcherBuilder<RArg1, RArg2, RArg3, ROut>, IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>>> Matches { get; init; }
    public required Func<DynamicAddress<OriginalProxy<ROut>>, DynamicAddress<ArgProxy<RArg1>>, DynamicAddress<ArgProxy<RArg2>>, DynamicAddress<ArgProxy<RArg3>>, IToken<ROut>> Definition { get; init; }
}