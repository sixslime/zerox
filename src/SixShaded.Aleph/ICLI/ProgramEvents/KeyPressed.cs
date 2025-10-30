namespace SixShaded.Aleph.ICLI.ProgramEvents;

internal class KeyPressed : IProgramEvent
{
    public required ConsoleKeyInfo KeyInfo { get; init; }
    
    public Task Handle(IProgramContext context)
    {
        return Console.Out.WriteAsync(KeyInfo.KeyChar);
    }
}