namespace SixShaded.FourZeroOne.Handles;

public interface ITokenContext
{
    public FZOSpec.IProcessorFZO.ITokenContext InternalValue { get; }
    public IMemory CurrentMemory { get; }
    public IInput Input { get; }
}