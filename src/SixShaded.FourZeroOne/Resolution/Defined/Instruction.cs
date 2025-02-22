#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Resolution.Defined
{
    using FourZeroOne.FZOSpec;
    using Handles;
    using SixShaded.FourZeroOne;
    using SixShaded.FourZeroOne.Resolution;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;

    public abstract record Instruction : Construct, IInstruction
    {
        public abstract IMemory TransformMemory(IMemory previousState);
        IMemoryFZO IInstruction.TransformMemoryUnsafe(IMemoryFZO memory) => TransformMemory(memory.ToHandle()).InternalValue;
        public override IEnumerable<IInstruction> Instructions => [this];
    }
}
