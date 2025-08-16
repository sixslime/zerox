namespace SixShaded.FourZeroOne.Handles;

public interface IInput
{
    public FZOSpec.IInputFZO InternalValue { get; }
    public ITask<int[]> ReadSelection(IHasElements<Rog> pool, int minCount, int maxCount);
}