namespace SixShaded.Aleph.Logical;

public interface IProgressor
{
    public string Identifier { get; }
    public Task Consume(IProgressionContext context);
}