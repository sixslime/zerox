namespace SixShaded.FourZeroOne.Core.Resolutions;

public sealed record MetaArgs<R1> : Resolution.Defined.NoOp
    where R1 : class, Res
{
    public required IOption<R1> Arg1 { get; init; }
    public override string ToString() => $"<{Arg1}>";
}
public sealed record MetaArgs<R1, R2> : Resolution.Defined.NoOp
    where R1 : class, Res
    where R2 : class, Res
{
    public required IOption<R1> Arg1 { get; init; }
    public required IOption<R2> Arg2 { get; init; }
    public override string ToString() => $"<{Arg1},{Arg2}>";
}
public sealed record MetaArgs<R1, R2, R3> : Resolution.Defined.NoOp
    where R1 : class, Res
    where R2 : class, Res
    where R3 : class, Res
{
    public required IOption<R1> Arg1 { get; init; }
    public required IOption<R2> Arg2 { get; init; }
    public required IOption<R3> Arg3 { get; init; }
    public override string ToString() => $"<{Arg1},{Arg2},{Arg3}>";
}