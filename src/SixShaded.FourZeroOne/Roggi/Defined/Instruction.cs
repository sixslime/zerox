namespace SixShaded.FourZeroOne.Roggi.Defined;

public abstract record Instruction : Roggi, IInstruction
{
    public abstract IMemory TransformMemory(IMemory previousState);
    FZOSpec.IMemoryFZO IInstruction.TransformMemoryUnsafe(FZOSpec.IMemoryFZO memory) => TransformMemory(memory.ToHandle()).InternalValue;
    public override IEnumerable<IInstruction> Instructions => [this];
}