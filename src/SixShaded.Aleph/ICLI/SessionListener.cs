namespace SixShaded.Aleph.ICLI;
using Logical;
internal class SessionListener : IDisposable
{
    public static SessionListener Link(IProgramContext context, Session session) => new(context, session);
    public IProgramContext LinkedProgram { get; }
    public Session Session { get; }
    private SessionListener(IProgramContext program, Session session)
    {
        LinkedProgram = program;
        Session = session;
        Session.TrackpointUpdatedEvent += TrackpointUpdatedListener;
        Session.SelectionPromptedEvent += SelectionPromptedListener;
        Session.SelectionCancelledEvent += SelectionCancelledListener;
    }

    private void TrackpointUpdatedListener(object? sender, TrackpointUpdatedEventArgs args)
    {
        AlephICLI.FireEventAndForget(
        new EProgramEvent.TrackpointUpdated()
        {
            Source = Session,
            Args = args
        });
    }
    private void SelectionPromptedListener(object? sender, SelectionPromptedEventArgs args)
    {
        AlephICLI.FireEventAndForget(
        new EProgramEvent.SelectionPrompted()
        {
            Source = Session,
            Args = args
        });
    }
    private void SelectionCancelledListener(object? sender, SelectionCancelledEventArgs args)
    {
        AlephICLI.FireEventAndForget(
        new EProgramEvent.SelectionCancelled()
        {
            Source = Session,
            Args = args
        });
    }
    public void Dispose()
    {
        Session.TrackpointUpdatedEvent -= TrackpointUpdatedListener;
        Session.SelectionPromptedEvent -= SelectionPromptedListener;
        Session.SelectionCancelledEvent -= SelectionCancelledListener;
    }
}
