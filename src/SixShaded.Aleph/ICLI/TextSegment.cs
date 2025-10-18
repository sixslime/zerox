namespace SixShaded.Aleph.ICLI;

internal record TextSegment
{
    public ConsoleColor? Background { get; init; } = null;
    public ConsoleColor? Foreground { get; init; } = null;
    public bool Bold { get; init; } = false;
    public bool Underline { get; init; } = false;
    public required string Text { get; init; }

}