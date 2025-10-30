namespace SixShaded.Aleph.ICLI.ProgramEvents;

internal class TrackpointUpdated : IProgramEvent
{
    public required Logical.Session Source { get; init; }
    public required Logical.TrackpointUpdatedEventArgs Args { get; init; }
    public Task Handle(IProgramContext context) => throw new NotImplementedException();
}