namespace SixShaded.FourZeroOne.Resolution.Defined;

public abstract record Instruction : Construct, IInstruction
{
    public abstract IMemory TransformMemory(IMemory previousState);
    FZOSpec.IMemoryFZO IInstruction.TransformMemoryUnsafe(FZOSpec.IMemoryFZO memory) => TransformMemory(memory.ToHandle()).InternalValue;
    public override IEnumerable<IInstruction> Instructions => [this];
}