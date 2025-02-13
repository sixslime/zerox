using MorseCode.ITask;
using Perfection;
using FourZeroOne.FZOSpec;
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using FourZeroOne.Macro.Unsafe;
using ResOpt = Perfection.IOption<FourZeroOne.Resolution.IResolution>;
#nullable enable
namespace Minima.FZO
{
    public record MinimaMemoryFZO : IMemoryFZO
    {
        private PMap<IMemoryAddress, IResolution> _objects;
        private PSequence<IRule> _rules;

        public MinimaMemoryFZO()
        {
            _objects = new();
            _rules = new();
        }

        IEnumerable<ITiple<IMemoryAddress, IResolution>> IMemoryFZO.Objects => _objects.Elements;
        IEnumerable<IRule> IMemoryFZO.Rules => _rules.Elements;
        
        IOption<R> IMemoryFZO.GetObject<R>(IMemoryAddress<R> address)
        {
            return _objects.At(address).RemapAs(x => (R)x);
        }

        IMemoryFZO IMemoryFZO.WithRules(IEnumerable<IRule> rules)
        {
            return this with { _rules = _rules.WithEntries(rules) };
        }

        IMemoryFZO IMemoryFZO.WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions)
        {
            return this with { _objects = _objects.WithEntries(insertions) };
        }

        IMemoryFZO IMemoryFZO.WithClearedAddresses(IEnumerable<IMemoryAddress> removals)
        {
            return this with { _objects = _objects.WithoutEntries(removals) };
        }

        
    }
}