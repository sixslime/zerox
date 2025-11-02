namespace SixShaded.Aleph.ICLI.ProgramEvents;
using State;
using Formatting;
internal class SessionAdded : IProgramEvent
{
    public required Logical.SessionAddedEventArgs Args { get; init; }

    public Task Handle(IProgramActions program)
    {
        ConsoleText.Text($"> New session added ({Args.Index})")
            .Format(TextFormat.Notification)
            .Print();
        program.SetState(
        state =>
            state with
            {
                Sessions =
                state.Sessions.WithEntries(
                new SessionInfo()
                {
                    SessionIndex = Args.Index
                })
            });
        return Task.CompletedTask;
    }
}