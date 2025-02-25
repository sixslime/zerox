namespace SixShaded.FourZeroOne.FZOSpec;

public interface IInputFZO
{
    public Task<int[]> GetSelection(IHasElements<Res> pool, int count);
}