
#nullable enable
namespace SixShaded.FourZeroOne.Handles
{
    using Rule = Rule.Unsafe.IRule<Res>;
    public interface IMemory
    {
        public FZOSpec.IMemoryFZO InternalValue { get; }
        public IEnumerable<ITiple<Addr, Res>> Objects { get; }
        public IEnumerable<Rule> Rules { get; }
        public IOption<R> GetObject<R>(IMemoryAddress<R> address) where R : class, Res;
        public IOption<Res> GetObjectUnsafe(Addr address);
        public IMemory WithRules(IEnumerable<Rule> rules);
        public IMemory WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) where R : class, Res;
        public IMemory WithObjectsUnsafe(IEnumerable<ITiple<Addr, Res>> insertions);
        public IMemory WithClearedAddresses(IEnumerable<Addr> removals);
    }
}