namespace SixShaded.FourZeroOne.Core.Roggis.Instructions;

public sealed record MellsanoAdd : Roggi.Defined.Instruction
{
    public required Mel Mellsano { get; init; }
    public override IMemory TransformMemory(IMemory state) => state.WithMellsanos([Mellsano]);
    public override string ToString() => $"<?>+{Mellsano}";
}