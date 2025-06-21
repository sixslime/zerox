namespace SixShaded.FourZeroOne.Core.Roggis.Instructions;

public sealed record Assign<D> : Roggi.Defined.Instruction
    where D : class, Rog
{
    public required IRoda<D> Address { get; init; }
    public required IOption<D> Subject { get; init; }

    public override IMemory TransformMemory(IMemory previousState) =>
        Subject.Check(out var data)
            ? previousState.WithObjects([(Address, data).Tiple()])
            : previousState.WithClearedAddresses([Address]);
    public override string ToString() => $"{Address}<-{Subject}";
}