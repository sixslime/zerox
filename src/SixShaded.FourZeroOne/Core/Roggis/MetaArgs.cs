namespace SixShaded.FourZeroOne.Core.Roggis;

using Roggi.Unsafe;
public sealed record MetaArgs<R1> : Roggi.Defined.NoOp, IMetaArgs
    where R1 : class, Rog
{
    public required IOption<R1> Arg1 { get; init; }
    public override string ToString() => $"<{Arg1}>";
    public RogOpt[] Args => [Arg1];
}

public sealed record MetaArgs<R1, R2> : Roggi.Defined.NoOp
    where R1 : class, Rog
    where R2 : class, Rog
{
    public required IOption<R1> Arg1 { get; init; }
    public required IOption<R2> Arg2 { get; init; }
    public override string ToString() => $"<{Arg1}, {Arg2}>";
    public RogOpt[] Args => [Arg1, Arg2];
}

public sealed record MetaArgs<R1, R2, R3> : Roggi.Defined.NoOp
    where R1 : class, Rog
    where R2 : class, Rog
    where R3 : class, Rog
{
    public required IOption<R1> Arg1 { get; init; }
    public required IOption<R2> Arg2 { get; init; }
    public required IOption<R3> Arg3 { get; init; }
    public RogOpt[] Args => [Arg1, Arg2, Arg3];
    public override string ToString() => $"<{Arg1}, {Arg2}, {Arg3}>";
}