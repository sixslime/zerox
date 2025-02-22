#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Resolution
{
    using FourZeroOne.FZOSpec;
    using Handles;
    using SixShaded.FourZeroOne;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;

    public interface IInstruction : IResolution
    {
        public IMemory TransformMemory(IMemory context);
        public IMemoryFZO TransformMemoryUnsafe(IMemoryFZO memory);
    }

}