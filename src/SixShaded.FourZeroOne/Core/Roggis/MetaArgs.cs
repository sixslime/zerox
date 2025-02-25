namespace SixShaded.FourZeroOne.Core.Roggis;

public sealed record MetaArgs<R1> : Roggi.Defined.NoOp
    where R1 : class, Rog
{
    public required IOption<R1> Arg1 { get; init; }
    public override string ToString() => $"<{Arg1}>";
}

public sealed record MetaArgs<R1, R2> : Roggi.Defined.NoOp
    where R1 : class, Rog
    where R2 : class, Rog
{
    public required IOption<R1> Arg1 { get; init; }
    public required IOption<R2> Arg2 { get; init; }
    public override string ToString() => $"<{Arg1},{Arg2}>";
}

public sealed record MetaArgs<R1, R2, R3> : Roggi.Defined.NoOp
    where R1 : class, Rog
    where R2 : class, Rog
    where R3 : class, Rog
{
    public required IOption<R1> Arg1 { get; init; }
    public required IOption<R2> Arg2 { get; init; }
    public required IOption<R3> Arg3 { get; init; }
    public override string ToString() => $"<{Arg1},{Arg2},{Arg3}>";
}