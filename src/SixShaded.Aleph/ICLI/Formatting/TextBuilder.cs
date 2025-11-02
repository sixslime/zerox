namespace SixShaded.Aleph.ICLI.Formatting;

// Console.WriteLine("\x1b[1mThis text is bold.\x1b[0m");
// Console.WriteLine("\x1b[4mThis text is underlined.\x1b[0m");
internal class TextBuilder
{
    private TextBuilder()
    { }

    private readonly List<TextSegment> _segments = [];
    public static TextBuilder Start() => new();
    public ConsoleText AsObject() => _segments.ToArray();
    public void Print() => AsObject().Print();
    public TextBuilder Text(string text)
    {
        _segments.Add(
        new()
        {
            Text = text,
            Format = TextFormat.Default,
        });
        return this;
    }

    public TextBuilder Format(TextFormat format)
    {
        if (_segments.Count == 0) return this;
        _segments[^1] =
            _segments[^1] with
            {
                Format = format
            };
        return this;
    }

    public TextBuilder Format(Func<TextFormat, TextFormat> formatChange)
    {
        if (_segments.Count == 0) return this;
        _segments[^1] =
            _segments[^1] with
            {
                Format = formatChange(_segments[^1].Format)
            };
        return this;
    }
}