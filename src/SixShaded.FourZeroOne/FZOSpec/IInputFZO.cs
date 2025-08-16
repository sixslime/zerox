namespace SixShaded.FourZeroOne.FZOSpec;

public interface IInputFZO
{
    public Task<int[]> GetSelection(Rog[] pool, int minCount, int maxCount);
}