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
        public override IEnumerable<ITiple<IStateAddress, IResolution>> Objects => _objects.Elements;
        public override IEnumerable<IRule> Rules => _rules.Elements;

        public Minimal()
        {
            _objects = new();
            _rules = new();
        }
        public override IState WithClearedAddresses(IEnumerable<IStateAddress> removals)
        {
            return this with { _objects = _objects.WithoutEntries(removals) };
        }
        public override IState WithObjectsUnsafe(IEnumerable<ITiple<IStateAddress, IResolution>> insertions)
        {
            return this with { _objects = _objects.WithEntries(insertions) };
        }
        public override IState WithRules(IEnumerable<IRule> rules)
        {
            return this with { _rules = _rules.WithEntries(rules) };
        }
        public override IOption<IResolution> GetObjectUnsafe(IStateAddress address)
        {
            return _objects.At(address);
        }

        private PMap<IStateAddress, IResolution> _objects;
        private PSequence<IRule> _rules;
    }
}
