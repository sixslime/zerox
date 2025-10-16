namespace SixShaded.Aleph.Logical;

public class Master
{
    private int _sessionIndex = -1;
    public IPSequence<Session> Sessions { get; private set; } = new PSequence<Session>();

    public event EventHandler<SessionSwitchedEventArgs>? SessionSwitchedEvent;

    public IOption<Session> CurrentSession => Sessions.At(_sessionIndex);

    private void NotifySwitchSession()
    {
        SessionSwitchedEvent?.Invoke(
        this, new()
        {
            Session = CurrentSession.Unwrap()
        });
    }
    public int AddSession(Session session)
    {
        Sessions = Sessions.WithEntries(session);
        return Sessions.Count - 1;
    }

    public bool SwitchSession(int index)
    {
        if (index > Sessions.Count - 1) return false;
        _sessionIndex = index;
        NotifySwitchSession();
        return true;
    }
}