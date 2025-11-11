namespace SixShaded.Aleph.ICLI;
using Formatting;
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
                    var currentState = session.GetLatestState(true);
                    var text = TextBuilder.Start();
                    text.Divider("operation stack");
                    foreach (var node in currentState.OperationStack.Reverse())
                    {
                        text.TranslateOperation(node)
                            .Text(" : ")
                            .Format(TextFormat.Structure);
                        foreach (var rogOpt in node.ArgRoggiStack.Reverse())
                        {
                            text.TranslateRogOpt(rogOpt)
                                .Text("; ")
                                .Format(TextFormat.Structure);
                        }
                        text.Text("\n");
                    }
                    text.Print();
                }
        };
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