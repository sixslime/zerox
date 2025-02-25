namespace SixShaded.FourZeroOne.Core.Roggis.Instructions;

public sealed record Assign<D> : Roggi.Defined.Instruction where D : class, Rog
{
    public required IMemoryAddress<D> Address { get; init; }
    public required D Subject { get; init; }
    public override IMemory TransformMemory(IMemory previousState) => previousState.WithObjects([(Address, Subject).Tiple()]);
    public override string ToString() => $"{Address}<-{Subject}";
}