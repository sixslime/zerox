
using Perfection;
using System;


#nullable enable
namespace FourZeroOne
{
    using ResObj = Resolution.IResolution;
    
    public interface IState
    {
        public IEnumerable<(Resolution.Unsafe.IStateAddress address, ResObj obj)> Objects { get; }
        public IEnumerable<Rule.IRule> Rules { get; }
        public IOption<R> GetObject<R>(Resolution.IStateAddress<R> address) where R : class, ResObj;
        public IState WithRules(IEnumerable<Rule.IRule> rules);
        public IState WithObjects<R>(IEnumerable<(Resolution.IStateAddress<R> address, R obj)> insertions) where R : class, ResObj;
        public IState WithClearedAddresses(IEnumerable<Resolution.Unsafe.IStateAddress> removals);
    }

    public static class _Extensions
    {
        public static IState WithResolution(this IState state, ResObj resolution)
        {
            return resolution.Instructions.AccumulateInto(state, (prevState, instruction) => instruction.ChangeState(prevState));
        }

    }
}