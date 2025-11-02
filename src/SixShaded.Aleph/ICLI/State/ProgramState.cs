namespace SixShaded.Aleph.ICLI.State;


internal record ProgramState
{
    public int SelectedSessionIndex { get; init; } = -1;
    public IPSequence<SessionInfo> Sessions { get; init; } = new PSequence<SessionInfo>();

}