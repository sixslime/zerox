namespace SixShaded.FourZeroOne.Core.Resolutions.Instructions;

public sealed record Assign<D> : Resolution.Defined.Instruction where D : class, Res
{
    public required IMemoryAddress<D> Address { get; init; }
    public required D Subject { get; init; }
    public override IMemory TransformMemory(IMemory previousState) => previousState.WithObjects([(Address, Subject).Tiple()]);
    public override string ToString() => $"{Address}<-{Subject}";
}