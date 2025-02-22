
#nullable enable
namespace SixShaded.FourZeroOne.Handles
{
    public interface IInput
    {
        public FZOSpec.IInputFZO InternalValue { get; }
        public ITask<int[]> ReadSelection(IHasElements<Res> pool, int count);
    }
}