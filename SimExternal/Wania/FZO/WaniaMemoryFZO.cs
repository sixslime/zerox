using MorseCode.ITask;
using Perfection;
using FourZeroOne.FZOSpec;
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using FourZeroOne.Macro.Unsafe;
using ResOpt = Perfection.IOption<FourZeroOne.Resolution.IResolution>;
#nullable enable
namespace Wania.FZO
{
    public record WaniaMemoryFZO : IMemoryFZO
    {
        public IEnumerable<ITiple<IStateAddress, IResolution>> Objects => _objects.Elements;
        public IEnumerable<IRule> Rules => _rules.Elements;

        public WaniaMemoryFZO()
        {
            _objects = new();
            _rules = new();
        }
        IOption<R> IMemoryFZO.GetObject<R>(IStateAddress<R> address)
        {
            return _objects.At(address).RemapAs(x => (R)x);
        }

        IMemoryFZO IMemoryFZO.WithRules(IEnumerable<IRule> rules)
        {
            return this with { _rules = _rules.WithEntries(rules) };
        }

        IMemoryFZO IMemoryFZO.WithObjects<R>(IEnumerable<ITiple<IStateAddress<R>, R>> insertions)
        {
            return this with { _objects = _objects.WithEntries(insertions) };
        }

        IMemoryFZO IMemoryFZO.WithClearedAddresses(IEnumerable<IStateAddress> removals)
        {
            return this with { _objects = _objects.WithoutEntries(removals) };
        }

        private PMap<IStateAddress, IResolution> _objects;
        private PSequence<IRule> _rules;
    }
}