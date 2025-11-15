namespace SixShaded.Aleph.ICLI.Formatting;

// Console.WriteLine("\x1b[1mThis text is bold.\x1b[0m");
// Console.WriteLine("\x1b[4mThis text is underlined.\x1b[0m");
internal class TextBuilder
{
    private TextBuilder()
    { }

    private readonly List<TextSegment> _segments = [];
    public static TextBuilder Start() => new();
    public ConsoleText Build() => _segments.ToArray();
    public void Print() => Build().Print();

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
    public TextBuilder AppendText(string text) =>
        TransformLastSegment(
        segment =>
            segment with
            {
                Text = segment.Text + text
            });

    public TextBuilder Format(TextFormat format) =>
        TransformLastSegment(
        segment =>
            segment with
            {
                Format = format
            });

    public TextBuilder Format(Func<TextFormat, TextFormat> formatChange) =>
        TransformLastSegment(
        segment =>
            segment with
            {
                Format = formatChange(segment.Format)
            });

    private TextBuilder TransformLastSegment(Func<TextSegment, TextSegment> func)
    {
        if (_segments.Count == 0) return this;
        _segments[^1] = func(_segments[^1]);
        return this;
    }
}