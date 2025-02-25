namespace SixShaded.FourZeroOne.Core.Roggis.Instructions;

public sealed record MellsanoAdd : Roggi.Defined.Instruction
{
    public required Mellsano.Unsafe.IMellsano<Rog> Mellsano { get; init; }
    public override IMemory TransformMemory(IMemory state) => state.WithMellsanos([Mellsano]);

    public override string ToString() => $"<?>+{Mellsano}";
}