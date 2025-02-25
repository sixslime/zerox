namespace SixShaded.FourZeroOne.FZOSpec;

public record FZOSource
{
    public required Kor Program { get; init; }
    public required IMemoryFZO InitialMemory { get; init; }
}