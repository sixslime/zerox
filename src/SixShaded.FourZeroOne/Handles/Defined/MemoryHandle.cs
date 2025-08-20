namespace SixShaded.FourZeroOne.Handles.Defined;

internal class MemoryHandle(FZOSpec.IMemoryFZO implementation) : IMemory
{
    private readonly FZOSpec.IMemoryFZO _implementation = implementation;
    FZOSpec.IMemoryFZO IMemory.InternalValue => _implementation;
    IEnumerable<ITiple<Addr, Rog>> IMemory.Objects => _implementation.Objects;
    IEnumerable<Mel> IMemory.Mellsanos => _implementation.Mellsanos;
    IEnumerable<ITiple<IRoveggi<D>, R>> IMemory.GetRovedanggiAssignmentsOfType<D, R>() => _implementation.GetRovedanggiAssignmentsOfType<D, R>();
    IOption<R> IMemory.GetObject<R>(IRoda<R> address) => _implementation.GetObject(address);
    RogOpt IMemory.GetObjectUnsafe(Addr address) => _implementation.GetObject(address);
    IMemory IMemory.WithClearedAddresses(IEnumerable<Addr> removals) => _implementation.WithClearedAddresses(removals).ToHandle();
    IMemory IMemory.WithObjects<R>(IEnumerable<ITiple<IRoda<R>, R>> insertions) => _implementation.WithObjects(insertions).ToHandle();
    IMemory IMemory.WithObjectsUnsafe(IEnumerable<ITiple<Addr, Rog>> insertions) => _implementation.WithObjects(insertions.Map(x => ((Addr)x.A, (Rog)x.B).Tiple())).ToHandle();
    IMemory IMemory.WithMellsanos(IEnumerable<Mel> mellsanos) => _implementation.WithMellsanos(mellsanos).ToHandle();
}