namespace SixShaded.Aleph.ICLI.Formatting;

internal static class Extensions
{
    public static TextBuilder Divider(this TextBuilder text, string title = "", int width = 24)
    {
        if (title.Length == 0) return text.Text(new string('-', width) + "\n").Format(TextFormat.Structure);

        width -= 6 + title.Length;
        return text.Text("--[ ")
            .Format(TextFormat.Structure)
            .Text(title.ToUpper())
            .Format(TextFormat.Title)
            .Text(" ]" + (width > 0 ? new string('-', width) : "--") + "\n")
            .Format(TextFormat.Structure);
    }
}