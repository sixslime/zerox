namespace SixShaded.Aleph.ICLI;

internal interface IEventSender
{
    public void SendEvent(IProgramEvent action);
    public void AddSessionListener(Logical.Session session);
}