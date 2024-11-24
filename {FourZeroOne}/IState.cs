
using Perfection;
using System;


#nullable enable
namespace FourZeroOne
{
    using ResObj = Resolution.IResolution;
    
    public interface IState
    {
        public IEnumerable<(Resolution.Unsafe.IStateAddress, ResObj)> Objects { get; }
        public IEnumerable<Rule.IRule> Rules { get; }
        public IOption<R> GetObject<R>(Resolution.IStateAddress<R> address) where R : class, ResObj;
        public IOption<ResObj> GetObjectUnsafe(Resolution.Unsafe.IStateAddress address);
        public IState WithRules(IEnumerable<Rule.IRule> rules);
        public IState WithObjects<R>(IEnumerable<(Resolution.IStateAddress<R>, R)> insertions) where R : class, ResObj;
        public IState WithObjectsUnsafe(IEnumerable<(Resolution.Unsafe.IStateAddress, ResObj)> insertions);
        public IState WithClearedAddresses(IEnumerable<Resolution.Unsafe.IStateAddress> removals);
    }

    // DEV - Perhaps have a "drivers" folder that has implementations for things like state, IO, etc. perhaps rethink the runtime model, splitting it into components that have drivers.

    public static class _Extensions
    {
        public static IState WithResolution(this IState state, ResObj resolution)
        {
            return resolution.Instructions.AccumulateInto(state, (prevState, instruction) => instruction.ChangeState(prevState));
        }

    }
}