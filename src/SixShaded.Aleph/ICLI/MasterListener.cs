namespace SixShaded.Aleph.ICLI;
using Logical;
using ProgramEvents;

internal class MasterListener : IDisposable
{
    public static MasterListener Link(IEventSender context, Master master) => new(context, master);
    public IEventSender LinkedProgram { get; }
    public Master Master { get; }
    private MasterListener(IEventSender program, Master instance)
    {
        LinkedProgram = program;
        Master = instance;
        Master.SessionAddedEvent += SessionAddedListener;
        Master.SessionSwitchedEvent += SessionSwitchedListener;
    }

    private void SessionAddedListener(object? sender, SessionAddedEventArgs args)
    {
        LinkedProgram.SendEvent(
        new SessionAdded()
        {
            Args = args,
        });
    }
    private void SessionSwitchedListener(object? sender, SessionSwitchedEventArgs args)
    {
        LinkedProgram.SendEvent(
        new SessionSwitched()
        {
            Args = args,
        });
    }
    public void Dispose()
    {
        Master.SessionAddedEvent -= SessionAddedListener;
        Master.SessionSwitchedEvent -= SessionSwitchedListener;
    }
}