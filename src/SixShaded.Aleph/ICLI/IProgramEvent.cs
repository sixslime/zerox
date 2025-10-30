namespace SixShaded.Aleph.ICLI;

public interface IProgramEvent
{
    public Task Handle(IProgramContext context);
}