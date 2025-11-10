namespace SixShaded.Aleph.ICLI.ProgramEvents;
using Formatting;
internal class SelectionCancelled : IProgramEvent
{
    public required Logical.Session Source { get; init; }
    public required Logical.SelectionCancelledEventArgs Args { get; init; }

    public Task Handle(IProgramActions actions)
    {
        ConsoleText.Text("Selection Cancelled (UNIMPLEMENTED)\n").Format(TextFormat.Error);
        return Task.CompletedTask;
    }
}