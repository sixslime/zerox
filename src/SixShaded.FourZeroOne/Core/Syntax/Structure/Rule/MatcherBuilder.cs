namespace SixShaded.FourZeroOne.Core.Syntax.Structure.Rule;

public sealed record MatcherBuilder<RVal> : IMatcherBuilder
    where RVal : class, Res
{ }
public sealed record MatcherBuilder<RArg1, ROut> : IMatcherBuilder
    where RArg1 : class, Res
    where ROut : class, Res
{ }
public sealed record MatcherBuilder<RArg1, RArg2, ROut> : IMatcherBuilder
    where RArg1 : class, Res
    where RArg2 : class, Res
    where ROut : class, Res
{ }
public sealed record MatcherBuilder<RArg1, RArg2, RArg3, ROut> : IMatcherBuilder
    where RArg1 : class, Res
    where RArg2 : class, Res
    where RArg3 : class, Res
    where ROut : class, Res
{ }