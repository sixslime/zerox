namespace SixShaded.Aleph.ICLI.ProgramEvents;

internal class SelectionPrompted : IProgramEvent
{
    public required Logical.Session Source { get; init; }
    public required Logical.SelectionPromptedEventArgs Args { get; init; }
    public Task Handle(IProgramActions context) => throw new NotImplementedException();
}