namespace SixShaded.FourZeroOne.Handles.Defined;

public class InputHandle(FZOSpec.IInputFZO implementation) : IInput
{
    private readonly FZOSpec.IInputFZO _implementation = implementation;
    FZOSpec.IInputFZO IInput.InternalValue => _implementation;
    ITask<int[]> IInput.ReadSelection(IHasElements<Rog> pool, int minCount, int maxCount) => _implementation.GetSelection(pool.ToArr(), minCount, maxCount).AsITask();
}