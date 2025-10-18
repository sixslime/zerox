namespace SixShaded.Aleph.ICLI;

// Console.WriteLine("\x1b[1mThis text is bold.\x1b[0m");
// Console.WriteLine("\x1b[4mThis text is underlined.\x1b[0m");
internal class TextBuilder
{
    private TextBuilder()
    { }

    private readonly List<TextSegment> _segments = [];
    public static TextBuilder Start() => new();
    public ConsoleText Build() => _segments.ToArray();
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

    public TextBuilder WithForeground(ConsoleColor? color)
    {
        if (_segments.Count == 0) return this;
        _segments[^1] =
            _segments[^1] with
            {
                Foreground = color
            };
        return this;
    }
    public TextBuilder WithBackround(ConsoleColor? color)
    {
        if (_segments.Count == 0) return this;
        _segments[^1] =
            _segments[^1] with
            {
                Background = color
            };
        return this;
    }
    public TextBuilder WithBold(bool value = true)
    {
        if (_segments.Count == 0) return this;
        _segments[^1] =
            _segments[^1] with
            {
                Bold = value
            };
        return this;
    }
    public TextBuilder WithUnderline(bool value = true)
    {
        if (_segments.Count == 0) return this;
        _segments[^1] =
            _segments[^1] with
            {
                Underline = value
            };
        return this;
    }
}