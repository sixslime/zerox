namespace SixShaded.Aleph.ICLI;

internal class ConsoleText(TextSegment[] segments)
{
    public static TextBuilder Build() => TextBuilder.Start();
    public TextSegment[] Segments { get; } = segments;

    public static implicit operator ConsoleText(TextSegment[] segments) => new(segments);
    public static implicit operator TextSegment[](ConsoleText text) => text.Segments;

    public void Print()
    {
        foreach (var segment in Segments)
        {
            if (segment.Foreground is not null) Console.ForegroundColor = (ConsoleColor)segment.Foreground;
            if (segment.Background is not null) Console.BackgroundColor = (ConsoleColor)segment.Background;
            string prefix = (segment.Bold ? "\x1b[1m" : "") + (segment.Underline ? "\x1b[4m" : "");
            string suffix = (segment.Bold ? "\x1b[0m" : "") + (segment.Underline ? "\x1b[0m" : "");
            Console.Out.Write($"{prefix}{segment.Text}{suffix}");
        }
        Console.ResetColor();
    }
}