namespace SixShaded.Aleph.ICLI;

internal interface IEventSender
{
    public void SendEvent(IProgramEvent action);
}