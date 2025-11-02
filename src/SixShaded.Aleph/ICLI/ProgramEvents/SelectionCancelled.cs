namespace SixShaded.Aleph.ICLI.ProgramEvents;

internal class SelectionCancelled : IProgramEvent
{
    public required Logical.Session Source { get; init; }
    public required Logical.SelectionCancelledEventArgs Args { get; init; }
    public Task Handle(IProgramActions actions) => throw new NotImplementedException();
}