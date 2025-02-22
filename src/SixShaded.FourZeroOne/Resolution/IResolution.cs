namespace SixShaded.FourZeroOne.Resolution;

public interface IResolution
{
    public IEnumerable<IInstruction> Instructions { get; }
}