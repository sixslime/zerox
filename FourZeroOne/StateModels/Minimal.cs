using System;
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using Perfection;

namespace FourZeroOne.StateModels
{
    // with the current implementations of PMap/PList, this is extremely inefficient, but we will fix later yah.
    public record Minimal : State
    {
        public override IEnumerable<(IStateAddress, IResolution)> Objects => _objects.Elements;
        public override IEnumerable<IRule> Rules => _rules.Elements;

        public Minimal()
        {
            _objects = new() { Elements = [] };
            _rules = new() { Elements = [] };
        }
        public override IState WithClearedAddresses(IEnumerable<IStateAddress> removals)
        {
            return this with { _objects = _objects with { dElements = E => E.ExceptBy(removals, x => x.key) } };
        }
        public override IState WithObjectsUnsafe(IEnumerable<(IStateAddress, IResolution)> insertions)
        {
            return this with { _objects = _objects with { dElements = E => E.Also(insertions) } };
        }
        public override IState WithRules(IEnumerable<IRule> rules)
        {
            return this with { _rules = _rules with { dElements = E => E.Also(rules) } };
        }
        public override IOption<IResolution> GetObjectUnsafe(IStateAddress address)
        {
            return _objects[address];
        }

        private PMap<IStateAddress, IResolution> _objects;
        private PList<IRule> _rules;
    }
}
