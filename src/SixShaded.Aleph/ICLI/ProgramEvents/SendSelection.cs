namespace SixShaded.Aleph.ICLI.ProgramEvents;

internal class SendSelection : IProgramEvent
{
    public required int[] Selection { get; init; }
    public Task Handle(IProgramContext context) => throw new NotImplementedException();
}