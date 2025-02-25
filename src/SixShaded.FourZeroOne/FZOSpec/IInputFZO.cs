namespace SixShaded.FourZeroOne.FZOSpec;

public interface IInputFZO
{
    public Task<int[]> GetSelection(IHasElements<Rog> pool, int count);
}