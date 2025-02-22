#nullable enable
namespace FourZeroOne.Resolution
{
    using FourZeroOne.FZOSpec;
    using Handles;
    using SixShaded.FourZeroOne;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;

    public interface IInstruction : IResolution
    {
        public IMemory TransformMemory(IMemory context);
        public FZOSpec.IMemoryFZO TransformMemoryUnsafe(FZOSpec.IMemoryFZO memory);
    }
    
}