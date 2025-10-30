namespace SixShaded.Aleph.ICLI.ProgramEvents;

internal class SessionSwitched : IProgramEvent
{
    public required Logical.SessionSwitchedEventArgs Args { get; init; }
    public Task Handle(IProgramContext context) => throw new NotImplementedException();
}