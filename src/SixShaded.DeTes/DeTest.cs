namespace SixShaded.DeTes;
public record DeTest : IDeTesTest
{
    public required IMemoryFZO InitialMemory { get; init; }
    public required DeTesDeclaration Declaration { get; init; }
}

