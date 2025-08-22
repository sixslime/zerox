namespace SixShaded.FourZeroOne.Core.Roggis.Instructions;

/// <summary>
/// intentionally keeps dynamic rodas.
/// </summary>
public sealed record LoadProgramState : Roggi.Defined.Instruction
{
    public required ProgramState State { get; init; }

    public override IMemory TransformMemory(IMemory previousState) => 
        State.Memory
            .WithClearedAddresses(State.Memory.Objects.FilterMap(x => x.A.MaybeA<ILoadOverwritingRoda<Rog>>()))
            .WithObjectsUnsafe(previousState.Objects.Where(x => x.A is ILoadOverwritingRoda<Rog>));
    public override string ToString() => $"LOAD({State})";
}