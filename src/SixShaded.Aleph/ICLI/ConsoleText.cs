namespace SixShaded.Aleph.ICLI;

internal class ConsoleText(TextSegment[] segments)
{
    public TextSegment[] Segments { get; } = segments;

    public static implicit operator ConsoleText(TextSegment[] segments) => new(segments);
    public static implicit operator TextSegment[](ConsoleText text) => text.Segments;
}