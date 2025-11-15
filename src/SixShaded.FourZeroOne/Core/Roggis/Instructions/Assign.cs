namespace SixShaded.FourZeroOne.Core.Roggis.Instructions;

public sealed record Assign<R> : Roggi.Defined.Instruction
    where R : class, Rog
{
    public required IRoda<R> Roda { get; init; }
    public required IOption<R> Data { get; init; }

    public override IMemory TransformMemory(IMemory previousState) =>
        Data.Check(out var dataValue)
            ? previousState.WithObjects([(Roda, dataValue).Tiple()])
            : previousState.WithClearedAddresses([Roda]);
    public override string ToString() => $"{Roda}<-{Data}";
}