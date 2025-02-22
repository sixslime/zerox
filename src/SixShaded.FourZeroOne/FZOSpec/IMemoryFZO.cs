
#nullable enable
namespace SixShaded.FourZeroOne.FZOSpec
{
    public interface IMemoryFZO
    {
        public IEnumerable<ITiple<Addr, Res>> Objects { get; }
        public IEnumerable<Rul> Rules { get; }
        public IEnumerable<ITiple<RuleID, int>> RuleMutes { get; }
        public IOption<R> GetObject<R>(IMemoryAddress<R> address) where R : class, Res;
        public int GetRuleMuteCount(RuleID ruleId);
        // Rules is an ordered sequence allowing duplicates.
        // 'WithRules' appends.
        // 'WithoutRules' removes the *first* instance of each rule in sequence.
        // Rules have a public ID assigned at creation, equality is based on ID.
        public IMemoryFZO WithRules(IEnumerable<Rul> rules);
        public IMemoryFZO WithRuleMutes(IEnumerable<RuleID> mutes);
        public IMemoryFZO WithoutRuleMutes(IEnumerable<RuleID> mutes);
        public IMemoryFZO WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) where R : class, Res;
        public IMemoryFZO WithClearedAddresses(IEnumerable<Addr> removals);
    }
   
}