#nullable enable
namespace SixShaded.FourZeroOne.Resolution
{
    public interface IInstruction : Res
    {
        public IMemory TransformMemory(IMemory context);
        public FZOSpec.IMemoryFZO TransformMemoryUnsafe(FZOSpec.IMemoryFZO memory);
    }
}