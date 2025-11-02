namespace SixShaded.Aleph.ICLI.ProgramEvents;

internal class KeyPressed : IProgramEvent
{
    public required ConsoleKeyInfo KeyInfo { get; init; }
    
    public Task Handle(IProgramActions actions)
    {
        actions.DoInput(KeyInfo);
    }
}