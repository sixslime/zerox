namespace SixShaded.Aleph.ICLI;

internal interface IProgramEvent
{
    public Task Handle(IProgramContext context);
}