namespace SixShaded.Aleph.ICLI;
using Formatting;
using FourZeroOne.Korvessa.Unsafe;
using Logical;
internal record InputAction
{
    public static InputAction Exit { get; } =
        new()
        {
            Name = "exit",
            Description = "Exit program.",
            ActionFunction = actions => actions.Exit(),
        };

    public static InputAction ShowCurrentOperationStack { get; } =
        new()
        {
            Name = "operation stack",
            Description = "Show the current session's operation stack.",
            ActionFunction =
                actions =>
                {
                    var session = actions.State.GetCurrentSession().GetLogicalSession();
                    var currentState = session.GetCurrentState(true);
                    var text = TextBuilder.Start();
                    text.Divider("operation stack");
                    var opStack = currentState.OperationStack.Reverse().ToArray();
                    foreach (var (i, node) in opStack.Enumerate())
                    {
                        text.Text(">").Format(TextFormat.Structure);

                        int resolvingIndex = node.ArgRoggiStack.Count();
                        if (node.Operation is IKorvessa<Rog> korvessa)
                            text.Text("$").Format(TextFormat.Hint with { Underline = true });
                        text.Text(" ");
                        bool isMetaExecute = Language.Builtin.Keys.StandardCoreKey.TYPEMATCH.GetKorssaTypeInfo(node.Operation).Match.Check(out var match) && match is CoreTypeMatcher.Types.Korssa.MetaExecuted;
                        foreach (var segment in Master.Instance.LanguageProvider.TranslateKorssa(node.Operation))
                        {

                            switch (segment)
                            {
                            case Language.Segments.TextSegment s:
                                text.Text(s.Text).Format(isMetaExecute ? TextFormat.Hint : TextFormat.Default with { Bold = true });
                                break;
                            case Language.Segments.KorssaArgSegment s:
                                if (resolvingIndex > s.Index)
                                    text.Text(".")
                                        .Format(TextFormat.Positive.Inverted());
                                else if (i+1 < opStack.Length && resolvingIndex == s.Index)
                                    text.Text("!").Format(TextFormat.Important.Inverted());
                                else
                                    text.Text("_")
                                        .Format(TextFormat.Negative)
                                        .Format(TextFormat.Negative.Inverted());
                                break;
                            default:
                                if (TryTranslateInline(segment).Check(out var str))
                                {
                                    text.Text(str).Format(TextFormat.Object with { Bold = true});
                                    break;
                                }
                                text.Text("?")
                                    .Format(
                                    TextFormat.Error with
                                    {
                                        Underline = true
                                    });
                                break;
                            }
                        }
                        foreach (var rogOpt in node.ArgRoggiStack.Reverse())
                        {
                            text.Text(" | ").Format(TextFormat.Structure);
                            var format = rogOpt.IsSome() ? TextFormat.Positive with { Bold = true } : TextFormat.Negative with { Bold = false };
                            if (rogOpt.Check(out var rog))
                            {
                                foreach (var segment in Master.Instance.LanguageProvider.TranslateRoggi(rog))
                                {
                                    switch (segment)
                                    {
                                    case Language.Segments.TextSegment s:
                                        text.Text(s.Text).Format(format);
                                        break;
                                    case Language.Segments.InlineKorssaSegment s:
                                        // TODO
                                        throw new NotImplementedException();
                                    }
                                }
                            }
                            else
                            {
                                text.Text(Master.Instance.LanguageProvider.TranslateNolla()).Format(format);
                            }
                        }
                        text.Text("\n");
                    }
                    text.Print();
                }
        };

    private static IOption<string> TryTranslateInline(Language.ITranslationSegment segment)
    {
        string? o = segment switch
        {
            Language.Segments.InlineRodaSegment s =>
                s.Value.ToString(),
            Language.Segments.InlineRoggiSegment s =>
                s.Value.RemapAs(x => x.ToString()).Or(Master.Instance.LanguageProvider.TranslateNolla()),
            Language.Segments.InlineRoggiTypeInfoSegment s =>
                Master.Instance.LanguageProvider.TranslateTypeInfo(s.Value),
            _ => null,
        };
        return o.NullToNone();
    }
    public static InputAction ShowCurrentOperationExpansions { get; } =
        new()
        {
            Name = "expansions",
            Description = "Show expansions of the currently selected operation.",
            ActionFunction = actions => { }
        };
    public static InputAction QuickInfo { get; } =
        new()
        {
            Name = "quick info",
            Description = "Show a quick overview of immediately relevant info.",
            ActionFunction = actions => { }
        };
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required Action<IProgramActions> ActionFunction { get; init; }
}