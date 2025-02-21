using MorseCode.ITask;
using FourZeroOne.FZOSpec;
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using ResOpt = SixShaded.NotRust.IOption<FourZeroOne.Resolution.IResolution>;
using SixShaded.NotRust;
#nullable enable
namespace SixShaded.MinimaFZO
{
    using Addr = IMemoryAddress<IResolution>;
    using Res = IResolution;
    using Rule = FourZeroOne.Rule.Unsafe.IRule<IResolution>;
    public record MinimaMemoryFZO : IMemoryFZO
    {
        private PMap<Addr, Res> _objects;
        private PSequence<Rule> _rules;
        private PMap<RuleID, int> _ruleMutes;

        public MinimaMemoryFZO()
        {
            _objects = new();
            _rules = new();
            _ruleMutes = new();
        }

        IEnumerable<ITiple<Addr, Res>> IMemoryFZO.Objects => _objects.Elements;
        IEnumerable<Rule> IMemoryFZO.Rules => _rules.Elements;
        IEnumerable<ITiple<RuleID, int>> IMemoryFZO.RuleMutes => _ruleMutes.Elements;

        IOption<R> IMemoryFZO.GetObject<R>(IMemoryAddress<R> address) => _objects.At(address).RemapAs(x => (R)x);
        int IMemoryFZO.GetRuleMuteCount(RuleID ruleId) => _ruleMutes.At(ruleId).Or(0);

        IMemoryFZO IMemoryFZO.WithRules(IEnumerable<Rule> rules) => this with
        {
            _rules = _rules.WithEntries(rules)
        };

        IMemoryFZO IMemoryFZO.WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) => this with
        {
            _objects = _objects.WithEntries(insertions)
        };

        IMemoryFZO IMemoryFZO.WithClearedAddresses(IEnumerable<Addr> removals) => this with
        {
            _objects = _objects.WithoutEntries(removals)
        };

        IMemoryFZO IMemoryFZO.WithRuleMutes(IEnumerable<RuleID> mutes) => this with
        {
            _ruleMutes = _ruleMutes.WithEntries(
                mutes.Map(mute =>
                    (mute, _ruleMutes.At(mute).Or(0) + 1))
                .Tipled())
        };

        IMemoryFZO IMemoryFZO.WithoutRuleMutes(IEnumerable<RuleID> mutes) => this with
        {
            _ruleMutes = mutes.AccumulateInto(_ruleMutes,
                (set, x) =>
                    set.WithEntries((x, set.At(x).Or(0) - 1).Tiple())
                    .WithoutEntries<PMap<RuleID, int>, RuleID>(set.At(x).Or(0) > 0 ? [] : [x]))
        };
    }
}