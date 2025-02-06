
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using Perfection;
using System;
using MorseCode.ITask;

#nullable enable
namespace FourZeroOne.Handles
{
    using ResObj = IResolution;
    
    public interface IMemory
    {
        public IEnumerable<ITiple<IStateAddress, ResObj>> Objects { get; }
        public IEnumerable<Rule.IRule> Rules { get; }
        public IOption<R> GetObject<R>(IStateAddress<R> address) where R : class, ResObj;
        public IOption<ResObj> GetObjectUnsafe(IStateAddress address);
        public IMemory WithRules(IEnumerable<Rule.IRule> rules);
        public IMemory WithObjects<R>(IEnumerable<ITiple<IStateAddress<R>, R>> insertions) where R : class, ResObj;
        public IMemory WithObjectsUnsafe(IEnumerable<ITiple<IStateAddress, ResObj>> insertions);
        public IMemory WithClearedAddresses(IEnumerable<IStateAddress> removals);
    }
    public interface ITokenContext
    {
        public IMemory CurrentMemory { get; }
        public IInput Input { get; }
    }
    public interface IInput
    {
        public ITask<int[]> ReadSelection(IHasElements<ResObj> pool, int count);
    }
    public class MemoryHandle(Logical.IMemory implementation) : IMemory
    {
        private readonly Logical.IMemory _implementation = implementation;
        IEnumerable<ITiple<IStateAddress, ResObj>> IMemory.Objects => _implementation.Objects;
        IEnumerable<IRule> IMemory.Rules => _implementation.Rules;
        IOption<R> IMemory.GetObject<R>(IStateAddress<R> address) => _implementation.GetObject(address);
        IOption<ResObj> IMemory.GetObjectUnsafe(IStateAddress address) => _implementation.GetObject((IStateAddress<ResObj>)address);
        IMemory IMemory.WithClearedAddresses(IEnumerable<IStateAddress> removals) => _implementation.WithClearedAddresses(removals).ToHandle();
        IMemory IMemory.WithObjects<R>(IEnumerable<ITiple<IStateAddress<R>, R>> insertions) => _implementation.WithObjects(insertions).ToHandle();
        IMemory IMemory.WithObjectsUnsafe(IEnumerable<ITiple<IStateAddress, ResObj>> insertions) => _implementation.WithObjects(insertions.Map(x => ((IStateAddress<ResObj>)x.A, (ResObj)x.B).Tiple())).ToHandle();
        IMemory IMemory.WithRules(IEnumerable<IRule> rules) => _implementation.WithRules(rules).ToHandle();
    }
    public class TokenContextHandle(Logical.IProcessor.ITokenContext implementation) : ITokenContext
    {
        private readonly Logical.IProcessor.ITokenContext _implementation = implementation;
        IMemory ITokenContext.CurrentMemory => _implementation.CurrentMemory.ToHandle();
        IInput ITokenContext.Input => _implementation.Input.ToHandle();
    }
    public class InputHandle(Logical.IInput implementation) : IInput
    {
        private readonly Logical.IInput _implementation = implementation;
        ITask<int[]> IInput.ReadSelection(IHasElements<ResObj> pool, int count) => _implementation.GetSelection(pool, count);
    }
    public static class Extensions
    {
        public static ITokenContext ToHandle(this Logical.IProcessor.ITokenContext implementation) => new TokenContextHandle(implementation);
        public static IMemory ToHandle(this Logical.IMemory implementation) => new MemoryHandle(implementation);
        public static IInput ToHandle(this Logical.IInput implementation) => new InputHandle(implementation);
        public static IMemory WithResolution(this IMemory state, ResObj resolution)
        {
            return resolution.Instructions.AccumulateInto(state, (prevState, instruction) => instruction.ChangeState(prevState));
        }

    }
}