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
        public IEnumerable<(IStateAddress, IResolution)> Objects => _objects.Elements;
        public IEnumerable<IRule> Rules => _rules.Elements;

        public Minimal()
        {
            _objects = new() { Elements = [] };
            _rules = new() { Elements = [] };
        }
        public IState WithClearedAddresses(IEnumerable<IStateAddress> removals)
        {
            return this with { _objects = _objects with { dElements = E => E.ExceptBy(removals, x => x.key) } };
        }
        public IState WithObjectsUnsafe(IEnumerable<(IStateAddress, IResolution)> insertions)
        {
            return this with { _objects = _objects with { dElements = E => E.Also(insertions) } };
        }
        public IState WithRules(IEnumerable<IRule> rules)
        {
            return this with { _rules = _rules with { dElements = E => E.Also(rules) } };
        }
        IOption<R> IState.GetObject<R>(IStateAddress<R> address)
        {
            return _objects[address].RemapAs(x => (R)x);
        }
        IState IState.WithObjects<R>(IEnumerable<(IStateAddress<R>, R)> insertions) => WithObjectsUnsafe(insertions.Map(x => ((IStateAddress)x.Item1, (IResolution)x.Item2)));

        private PMap<IStateAddress, IResolution> _objects;
        private PList<IRule> _rules;
    }
}
