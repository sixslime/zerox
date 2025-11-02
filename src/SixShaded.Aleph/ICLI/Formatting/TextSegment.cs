namespace SixShaded.Aleph.ICLI.Formatting;
internal record TextSegment
{
    public required TextFormat Format { get; init; } = new();
    public required string Text { get; init; }
}