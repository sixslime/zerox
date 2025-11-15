namespace SixShaded.Aleph.ICLI.ProgramEvents;
using Formatting;
internal class SelectionPrompted : IProgramEvent
{
    public required Logical.Session Source { get; init; }
    public required Logical.SelectionPromptedEventArgs Args { get; init; }

    public Task Handle(IProgramActions actions)
    {
        ConsoleText.Text("Selection Prompted (UNIMPLEMENTED)\n").Format(TextFormat.Error).Print();
        return Task.CompletedTask;
    }
}