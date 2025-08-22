namespace SixShaded.FourZeroOne.Core.Roggis;

public sealed record ProgramState : Roggi.Defined.NoOp
{
    public required IMemory Memory { get; init; }

    public override string ToString() => $"STATE({Memory.GetHashCode().ToBase("YFPGCRLAOEUIDHNS_ZQJKXBMWV- ")[0..10]})";
}