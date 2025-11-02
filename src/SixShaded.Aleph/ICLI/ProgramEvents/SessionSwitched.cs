namespace SixShaded.Aleph.ICLI.ProgramEvents;
using State;
using Formatting;
internal class SessionSwitched : IProgramEvent
{
    public required Logical.SessionSwitchedEventArgs Args { get; init; }

    public Task Handle(IProgramActions program)
    {
        ConsoleText.Text("> Switched To Session ")
            .Format(TextFormat.Notification)
            .Text(Args.Index.ToString())
            .Format(
            TextFormat.Notification with
            {
                Underline = true,
                Bold = true
            })
            .Print();
        return Task.CompletedTask;
    }
}