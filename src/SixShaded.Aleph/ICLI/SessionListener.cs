namespace SixShaded.Aleph.ICLI;
using Logical;
using ProgramEvents;

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
        LinkedProgram.SendEvent(
        new TrackpointUpdated()
        {
            Args = args,
            Source = Session,
        });
    }
    private void SelectionPromptedListener(object? sender, SelectionPromptedEventArgs args)
    {
        LinkedProgram.SendEvent(
        new SelectionPrompted()
        {
            Args = args,
            Source = Session,
        });
    }
    private void SelectionCancelledListener(object? sender, SelectionCancelledEventArgs args)
    {
        LinkedProgram.SendEvent(
        new SelectionCancelled()
        {
            Args = args,
            Source = Session,
        });
    }
    public void Dispose()
    {
        Session.TrackpointUpdatedEvent -= TrackpointUpdatedListener;
        Session.SelectionPromptedEvent -= SelectionPromptedListener;
        Session.SelectionCancelledEvent -= SelectionCancelledListener;
    }
}
