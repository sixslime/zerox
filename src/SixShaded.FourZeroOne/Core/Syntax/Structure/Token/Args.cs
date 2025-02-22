namespace SixShaded.FourZeroOne.Core.Syntax.Structure.Token;

public sealed record Args<RArg1>
    where RArg1 : class, Res
{
    public required IToken<RArg1> A { get; init; }
}
public sealed record Args<RArg1, RArg2>
    where RArg1 : class, Res
    where RArg2 : class, Res
{
    public required IToken<RArg1> A { get; init; }
    public required IToken<RArg2> B { get; init; }
}
public sealed record Args<RArg1, RArg2, RArg3>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where RArg3 : class, Res
{
    public required IToken<RArg1> A { get; init; }
    public required IToken<RArg2> B { get; init; }
    public required IToken<RArg3> C { get; init; }
}