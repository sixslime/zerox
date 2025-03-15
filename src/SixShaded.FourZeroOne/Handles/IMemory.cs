namespace SixShaded.FourZeroOne.Handles;

public interface IMemory
{
    public FZOSpec.IMemoryFZO InternalValue { get; }
    public IEnumerable<ITiple<IRoda<>, Rog>> Objects { get; }
    public IEnumerable<Mel> Mellsanos { get; }

    public IOption<R> GetObject<R>(IRoda<R> address)
        where R : class, Rog;

    public IOption<Rog> GetObjectUnsafe(IRoda<> address);
    public IMemory WithMellsanos(IEnumerable<Mel> mellsanos);

    public IMemory WithObjects<R>(IEnumerable<ITiple<IRoda<R>, R>> insertions)
        where R : class, Rog;

    public IMemory WithObjectsUnsafe(IEnumerable<ITiple<IRoda<>, Rog>> insertions);
    public IMemory WithClearedAddresses(IEnumerable<IRoda<>> removals);
}