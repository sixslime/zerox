
#nullable enable
namespace SixShaded.FourZeroOne.FZOSpec
{
    public record FZOSource
    {
        public required Tok Program { get; init; }
        public required IMemoryFZO InitialMemory { get; init; }
    }
}