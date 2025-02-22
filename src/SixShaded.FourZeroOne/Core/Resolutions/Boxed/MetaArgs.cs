#nullable enable
namespace FourZeroOne.Core.Resolutions.Boxed
{
    public sealed record MetaArgs<R1> : NoOp
        where R1 : class, ResObj
    {
        public required IOption<R1> Arg1 { get; init; }
        public override string ToString() => $"<{Arg1}>";
    }
    public sealed record MetaArgs<R1, R2> : NoOp
        where R1 : class, ResObj
        where R2 : class, ResObj
    {
        public required IOption<R1> Arg1 { get; init; }
        public required IOption<R2> Arg2 { get; init; }
        public override string ToString() => $"<{Arg1},{Arg2}>";
    }
    public sealed record MetaArgs<R1, R2, R3> : NoOp
        where R1 : class, ResObj
        where R2 : class, ResObj
        where R3 : class, ResObj
    {
        public required IOption<R1> Arg1 { get; init; }
        public required IOption<R2> Arg2 { get; init; }
        public required IOption<R3> Arg3 { get; init; }
        public override string ToString() => $"<{Arg1},{Arg2},{Arg3}>";
    }
}