namespace SixShaded.Aleph.Logical;

public interface IProgressionContext
{
    public bool IsBackward { get; }
    public Task<IOption<Trackpoint.Step>> Next();
}