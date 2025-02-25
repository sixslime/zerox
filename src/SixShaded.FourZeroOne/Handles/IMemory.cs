namespace SixShaded.FourZeroOne.Handles;

public interface IMemory
{
    public FZOSpec.IMemoryFZO InternalValue { get; }
    public IEnumerable<ITiple<Addr, Rog>> Objects { get; }
    public IEnumerable<Mel> Mellsanos { get; }
    public IOption<R> GetObject<R>(IMemoryAddress<R> address) where R : class, Rog;
    public IOption<Rog> GetObjectUnsafe(Addr address);
    public IMemory WithMellsanos(IEnumerable<Mel> mellsanos);
    public IMemory WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) where R : class, Rog;
    public IMemory WithObjectsUnsafe(IEnumerable<ITiple<Addr, Rog>> insertions);
    public IMemory WithClearedAddresses(IEnumerable<Addr> removals);
}