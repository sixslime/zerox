namespace SixShaded.Aleph.ICLI.State;


internal record ProgramState
{
    public IPSequence<SessionInfo> Sessions { get; init; } = new PSequence<SessionInfo>();

}