namespace SixShaded.Aleph.ICLI.State;


internal record ProgramState
{
    public int SelectedSession { get; init; } = -1;
    public IPSequence<SessionInfo> Sessions { get; init; } = new PSequence<SessionInfo>();
    public IPSequence<SessionInfo> HiddenSessions { get; init; } = new PSequence<SessionInfo>();
}