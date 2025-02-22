
#nullable enable
namespace SixShaded.FourZeroOne.FZOSpec
{
    public interface IInputFZO
    {
        public ITask<int[]> GetSelection(IHasElements<Res> pool, int count);
    }
}