namespace SixShaded.FourZeroOne.Core.Syntax.Structure.Korssa;

public sealed record Args<RArg1>
    where RArg1 : class, Rog
{
    public required IKorssa<RArg1> A { get; init; }
}

public sealed record Args<RArg1, RArg2>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
{
    public required IKorssa<RArg1> A { get; init; }
    public required IKorssa<RArg2> B { get; init; }
}

public sealed record Args<RArg1, RArg2, RArg3>
    where RArg1 : class, Rog
    where RArg2 : class, Rog
    where RArg3 : class, Rog
{
    public required IKorssa<RArg1> A { get; init; }
    public required IKorssa<RArg2> B { get; init; }
    public required IKorssa<RArg3> C { get; init; }
}