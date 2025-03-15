namespace SixShaded.FourZeroOne.Handles.Defined;

public class InputHandle(FZOSpec.IInputFZO implementation) : IInput
{
    private readonly FZOSpec.IInputFZO _implementation = implementation;
    FZOSpec.IInputFZO IInput.InternalValue => _implementation;
    ITask<int[]> IInput.ReadSelection(IHasElements<Rog> pool, int count) => _implementation.GetSelection(pool.ToArr(), count).AsITask();
}