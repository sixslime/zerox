namespace SixShaded.Aleph.ICLI.Formatting;
internal class ConsoleText(TextSegment[] segments)
{
    public static TextBuilder Text(string text) => TextBuilder.Start().Text(text);
    public TextSegment[] Segments { get; } = segments;

    public static implicit operator ConsoleText(TextSegment[] segments) => new(segments);
    public static implicit operator TextSegment[](ConsoleText text) => text.Segments;
    public static ConsoleText operator +(ConsoleText a, ConsoleText b) =>
        new(
        [
            ..a.Segments,
            ..b.Segments
        ]);

    public ConsoleText Append(ConsoleText other) => this + other;
    public void Print()
    {
        foreach (var segment in Segments)
        {
            var format = segment.Format;
            if (format.Foreground is not null) Console.ForegroundColor = (ConsoleColor)format.Foreground;
            if (format.Background is not null) Console.BackgroundColor = (ConsoleColor)format.Background;
            string prefix = (format.Bold ? "\x1b[1m" : "") + (format.Underline ? "\x1b[4m" : "");
            string suffix = (format.Bold ? "\x1b[0m" : "") + (format.Underline ? "\x1b[0m" : "");
            Console.Out.Write($"{prefix}{segment.Text}{suffix}");
            Console.ResetColor();
        }
    }

}