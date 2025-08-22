namespace SixShaded.FourZeroOne.Core.Roggis.Instructions;

public sealed record LoadProgramState : Roggi.Defined.Instruction
{
    public required ProgramState State { get; init; }

    public override IMemory TransformMemory(IMemory previousState) => State.Memory;
    public override string ToString() => $"LOAD({State})";
}