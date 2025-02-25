namespace SixShaded.FourZeroOne.Roggi;

public interface IInstruction : Rog
{
    public IMemory TransformMemory(IMemory context);
    public FZOSpec.IMemoryFZO TransformMemoryUnsafe(FZOSpec.IMemoryFZO memory);
}