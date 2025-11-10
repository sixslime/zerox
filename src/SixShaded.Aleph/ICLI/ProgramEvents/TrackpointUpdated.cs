namespace SixShaded.Aleph.ICLI.ProgramEvents;
using Formatting;
internal class TrackpointUpdated : IProgramEvent
{
    public required Logical.Session Source { get; init; }
    public required Logical.TrackpointUpdatedEventArgs Args { get; init; }

    public Task Handle(IProgramActions actions)
    {
        ConsoleText.Text("Selection Prompted (UNIMPLEMENTED)\n").Format(TextFormat.Error);
        return Task.CompletedTask;
    }
}