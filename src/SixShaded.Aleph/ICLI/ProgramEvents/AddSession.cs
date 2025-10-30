namespace SixShaded.Aleph.ICLI.ProgramEvents;

internal class AddSession : IProgramEvent
{
    public required IStateFZO RootState { get; init; }
    public Task Handle(IProgramContext context) => throw new NotImplementedException();
}