namespace SixShaded.FourZeroOne.Handles;

public interface IKorssaContext
{
    public FZOSpec.IProcessorFZO.IKorssaContext InternalValue { get; }
    public IMemory CurrentMemory { get; }
    public IInput Input { get; }
}