using System;
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using Perfection;

namespace FourZeroOne.StateModels
{
    // with the current implementations of PMap/PList, this is extremely inefficient, but we will fix later yah.
    public record Minimal : IState
    {
        public required PMap<IStateAddress, IResolution> ObjectMap { get; init; } 
        public Updater<PMap<IStateAddress, IResolution>> dObjectMap { init => ObjectMap = value(ObjectMap); }
        public required PList<IRule> RuleList { get; init; } 
        public Updater<PList<IRule>> dRuleList { init => RuleList = value(RuleList); }
        public IEnumerable<(IStateAddress, IResolution)> Objects => ObjectMap.Elements;
        public IEnumerable<IRule> Rules => RuleList.Elements;

        public IState WithClearedAddresses(IEnumerable<IStateAddress> removals)
        {
            return this with { dObjectMap = Q => Q with { dElements = E => E.ExceptBy(removals, x => x.key) } };
        }

        public IState WithObjectsUnsafe(IEnumerable<(IStateAddress, IResolution)> insertions)
        {
            return this with { dObjectMap = Q => Q with { dElements = E => E.Also(insertions) } };
        }

        public IState WithRules(IEnumerable<IRule> rules)
        {
            return this with { dRuleList = Q => Q with { dElements = E => E.Also(rules) } };
        }

        IOption<R> IState.GetObject<R>(IStateAddress<R> address)
        {
            return ObjectMap[address].NullToNone().RemapAs(x => (R)x);
        }

        IState IState.WithObjects<R>(IEnumerable<(IStateAddress<R>, R)> insertions) => WithObjectsUnsafe(insertions.Map(x => ((IStateAddress)x.Item1, (IResolution)x.Item2)));
    }
}
