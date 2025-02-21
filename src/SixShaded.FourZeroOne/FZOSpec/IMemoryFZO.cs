using MorseCode.ITask;
using SixShaded.FourZeroOne;
using SixShaded.NotRust;
using SixShaded.SixLib.GFunc;
#nullable enable
namespace SixShaded.FourZeroOne.FZOSpec
{
    using Token;
    using Res = Resolution.IResolution;
    using Rule = Rule.Unsafe.IRule<Resolution.IResolution>;
    using ResOpt = IOption<Resolution.IResolution>;
    using Token = IToken<Resolution.IResolution>;
    using Addr = Resolution.IMemoryAddress<Resolution.IResolution>;
    using Resolution;
    using Resolution.Unsafe;
    using Rule;
    using System.Diagnostics.CodeAnalysis;
    using SixShaded.NotRust;
   
    public interface IMemoryFZO
    {
        public IEnumerable<ITiple<Addr, Res>> Objects { get; }
        public IEnumerable<Rule> Rules { get; }
        public IEnumerable<ITiple<RuleID, int>> RuleMutes { get; }
        public IOption<R> GetObject<R>(IMemoryAddress<R> address) where R : class, Res;
        public int GetRuleMuteCount(RuleID ruleId);
        // Rules is an ordered sequence allowing duplicates.
        // 'WithRules' appends.
        // 'WithoutRules' removes the *first* instance of each rule in sequence.
        // Rules have a public ID assigned at creation, equality is based on ID.
        public IMemoryFZO WithRules(IEnumerable<Rule> rules);
        public IMemoryFZO WithRuleMutes(IEnumerable<RuleID> mutes);
        public IMemoryFZO WithoutRuleMutes(IEnumerable<RuleID> mutes);
        public IMemoryFZO WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) where R : class, Res;
        public IMemoryFZO WithClearedAddresses(IEnumerable<Addr> removals);
    }
   
}