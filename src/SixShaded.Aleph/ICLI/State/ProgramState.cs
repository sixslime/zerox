namespace SixShaded.Aleph.ICLI.State;


internal record ProgramState
{
    public int SelectedSession { get; init; } = -1;
    public IPSequence<SessionInfo> Sessions { get; init; } = new PSequence<SessionInfo>();
    public IPSequence<SessionInfo> HiddenSessions { get; init; } = new PSequence<SessionInfo>();

    public ProgramState WithCurrentSession(Func<SessionInfo, SessionInfo> changeFunc)
    {
        if (SelectedSession < 0) return this;
        return this with
        {
            Sessions = Sessions.WithEntries(((Index)SelectedSession, changeFunc(GetCurrentSession())).Tiple())
        };
    }
    public SessionInfo GetCurrentSession() => Sessions.At(SelectedSession).Expect("No current session?");
}