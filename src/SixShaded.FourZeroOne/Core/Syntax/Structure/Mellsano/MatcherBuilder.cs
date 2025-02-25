namespace SixShaded.FourZeroOne.Core.Syntax.Structure.Mellsano;

public sealed record MatcherBuilder<RVal> : IMatcherBuilder
    where RVal : class, Rog
{ }

public sealed record MatcherBuilder<RArg1, ROut> : IMatcherBuilder
    where RArg1 : class, Rog
    where ROut : class, Rog
{ }

public sealed record MatcherBuilder<RArg1, RArg2, ROut> : IMatcherBuilder
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where ROut : class, Rog
{ }

public sealed record MatcherBuilder<RArg1, RArg2, RArg3, ROut> : IMatcherBuilder
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
    where ROut : class, Rog
{ }