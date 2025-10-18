namespace SixShaded.Aleph.ICLI;
using Logical;
internal class MasterListener : IDisposable
{
    public Master Master { get; }
    public MasterListener(Master instance)
    {
        Master = instance;
        Master.SessionAddedEvent += SessionAddedListener;
        Master.SessionSwitchedEvent += SessionSwitchedListener;
    }

    private void SessionAddedListener(object? sender, SessionAddedEventArgs args)
    {
        AlephICLI.FireEventAndForget(
        new EProgramEvent.SessionAdded()
        {
            Args = args
        });
    }
    private void SessionSwitchedListener(object? sender, SessionSwitchedEventArgs args)
    {
        AlephICLI.FireEventAndForget(
        new EProgramEvent.SessionSwitched()
        {
            Args = args
        });
    }
    public void Dispose()
    {
        Master.SessionAddedEvent -= SessionAddedListener;
        Master.SessionSwitchedEvent -= SessionSwitchedListener;
    }
}