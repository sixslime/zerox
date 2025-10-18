namespace SixShaded.Aleph.ICLI;

// Console.WriteLine("\x1b[1mThis text is bold.\x1b[0m");
// Console.WriteLine("\x1b[4mThis text is underlined.\x1b[0m");
internal class TextBuilder
{
    private TextBuilder()
    { }

    private readonly List<TextSegment> _segments = [];
    public static TextBuilder Start() => new();
    public TextSegment[] Build() => _segments.ToArray();
    public TextBuilder Text(string text, ConsoleColor? foreground = null, ConsoleColor? background = null, bool bold = false, bool underline = false)
    {
        _segments.Add(
        new()
        {
            Text = text,
            Background = background,
            Foreground = foreground,
            Bold = bold,
            Underline = bold,
        });
        return this;
    }

}