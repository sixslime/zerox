namespace SixShaded.Aleph.ICLI.Formatting;
using FourZeroOne.FZOSpec;
using Logical;
using Language;
internal static class Extensions
{
    private static LanguageProvider _languageProvider => Master.Instance.LanguageProvider;
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

    public static TextBuilder TranslateOperation(this TextBuilder text, IStateFZO.IOperationNode node)
    {
        int resolvingIndex = node.ArgRoggiStack.Count();
        foreach (var segment in _languageProvider.TranslateKorssa(node.Operation))
        {
            switch (segment)
            {
            case Language.Segments.TextSegment s:
                text.Text(s.Text).Format(TextFormat.Object);
                break;
            case Language.Segments.KorssaArgSegment s:
                if (resolvingIndex > s.Index)
                    text.Text("*")
                        .Format(TextFormat.Positive with
                        {
                            Background = ConsoleColor.DarkGray
                        });
                else if (resolvingIndex == s.Index)
                    text.Text("#")
                        .Format(TextFormat.Title with
                        {
                            Background = ConsoleColor.DarkGray
                        });
                else
                    text.Text(".")
                        .Format(TextFormat.Negative)
                        .Format(
                        TextFormat.Negative with
                        {
                            Background = ConsoleColor.DarkGray
                        });
                break;
            case Language.Segments.InlineTranslationSegment s:
                // TODO
                throw new NotImplementedException();
            }
        }
        return text;
    }
}