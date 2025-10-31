namespace SixShaded.Aleph.ICLI.ProgramEvents;

internal class SessionAdded : IProgramEvent
{
    public required Logical.SessionAddedEventArgs Args { get; init; }

    public Task Handle(IEventSender context)
    {

    }
}