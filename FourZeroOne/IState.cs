
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
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
    public abstract record State : IState
    {
        public abstract IEnumerable<(IStateAddress, ResObj)> Objects { get; }
        public abstract IEnumerable<IRule> Rules { get; }

        public abstract IOption<ResObj> GetObjectUnsafe(IStateAddress address);
        public abstract IState WithClearedAddresses(IEnumerable<IStateAddress> removals);
        public abstract IState WithObjectsUnsafe(IEnumerable<(IStateAddress, ResObj)> insertions);
        public abstract IState WithRules(IEnumerable<IRule> rules);

        // this kinda blows ass, but like whatever right.
        public virtual bool Equals(State? other)
        {
            return (other is not null) &&
                (Objects.ToHashSet() == other.Objects.ToHashSet()) &&
                Rules.SequenceEqual(other.Rules);
        }
        public override int GetHashCode()
        {
            return Objects.ToHashSet().GetHashCode() * Rules.GetHashCode();
        }
        IOption<R> IState.GetObject<R>(IStateAddress<R> address) => GetObjectUnsafe(address).RemapAs(x => (R)x);
        IState IState.WithObjects<R>(IEnumerable<(IStateAddress<R>, R)> insertions) => WithObjectsUnsafe(insertions.Map(x => ((IStateAddress)x.Item1, (ResObj)x.Item2)));
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