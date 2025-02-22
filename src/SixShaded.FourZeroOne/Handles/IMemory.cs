
#nullable enable
namespace SixShaded.FourZeroOne.Handles
{
    public interface IMemory
    {
        public FZOSpec.IMemoryFZO InternalValue { get; }
        public IEnumerable<ITiple<Addr, Res>> Objects { get; }
        public IEnumerable<Rul> Rules { get; }
        public IOption<R> GetObject<R>(IMemoryAddress<R> address) where R : class, Res;
        public IOption<Res> GetObjectUnsafe(Addr address);
        public IMemory WithRules(IEnumerable<Rul> rules);
        public IMemory WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) where R : class, Res;
        public IMemory WithObjectsUnsafe(IEnumerable<ITiple<Addr, Res>> insertions);
        public IMemory WithClearedAddresses(IEnumerable<Addr> removals);
    }
}