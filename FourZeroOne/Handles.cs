
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using Perfection;
using System;
using MorseCode.ITask;
using FourZeroOne.FZOSpec;

#nullable enable
namespace FourZeroOne.Handles
{
    using ResObj = IResolution;
    
    public interface IMemory
    {
        public FZOSpec.IMemoryFZO InternalValue { get; }
        public IEnumerable<ITiple<IMemoryAddress, ResObj>> Objects { get; }
        public IEnumerable<Rule.IRule> Rules { get; }
        public IOption<R> GetObject<R>(IMemoryAddress<R> address) where R : class, ResObj;
        public IOption<ResObj> GetObjectUnsafe(IMemoryAddress address);
        public IMemory WithRules(IEnumerable<Rule.IRule> rules);
        public IMemory WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) where R : class, ResObj;
        public IMemory WithObjectsUnsafe(IEnumerable<ITiple<IMemoryAddress, ResObj>> insertions);
        public IMemory WithClearedAddresses(IEnumerable<IMemoryAddress> removals);
    }
    public interface ITokenContext
    {
        public FZOSpec.IProcessorFZO.ITokenContext InternalValue { get; }
        public IMemory CurrentMemory { get; }
        public IInput Input { get; }
    }
    public interface IInput
    {
        public FZOSpec.IInputFZO InternalValue { get; }
        public ITask<int[]> ReadSelection(IHasElements<ResObj> pool, int count);
    }
    public class MemoryHandle(FZOSpec.IMemoryFZO implementation) : IMemory
    {
        private readonly FZOSpec.IMemoryFZO _implementation = implementation;
        IMemoryFZO IMemory.InternalValue => _implementation;
        IEnumerable<ITiple<IMemoryAddress, ResObj>> IMemory.Objects => _implementation.Objects;
        IEnumerable<IRule> IMemory.Rules => _implementation.Rules;
        IOption<R> IMemory.GetObject<R>(IMemoryAddress<R> address) => _implementation.GetObject(address);
        IOption<ResObj> IMemory.GetObjectUnsafe(IMemoryAddress address) => _implementation.GetObject((IMemoryAddress<ResObj>)address);
        IMemory IMemory.WithClearedAddresses(IEnumerable<IMemoryAddress> removals) => _implementation.WithClearedAddresses(removals).ToHandle();
        IMemory IMemory.WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) => _implementation.WithObjects(insertions).ToHandle();
        IMemory IMemory.WithObjectsUnsafe(IEnumerable<ITiple<IMemoryAddress, ResObj>> insertions) => _implementation.WithObjects(insertions.Map(x => ((IMemoryAddress<ResObj>)x.A, (ResObj)x.B).Tiple())).ToHandle();
        IMemory IMemory.WithRules(IEnumerable<IRule> rules) => _implementation.WithRules(rules).ToHandle();
    }
    public class TokenContextHandle(FZOSpec.IProcessorFZO.ITokenContext implementation) : ITokenContext
    {
        private readonly FZOSpec.IProcessorFZO.ITokenContext _implementation = implementation;
        IMemory ITokenContext.CurrentMemory => _implementation.CurrentMemory.ToHandle();
        IInput ITokenContext.Input => _implementation.Input.ToHandle();

        IProcessorFZO.ITokenContext ITokenContext.InternalValue => _implementation;
    }
    public class InputHandle(FZOSpec.IInputFZO implementation) : IInput
    {
        private readonly FZOSpec.IInputFZO _implementation = implementation;

        IInputFZO IInput.InternalValue => _implementation;

        ITask<int[]> IInput.ReadSelection(IHasElements<ResObj> pool, int count) => _implementation.GetSelection(pool, count);
    }
    public static class Extensions
    {
        public static ITokenContext ToHandle(this FZOSpec.IProcessorFZO.ITokenContext implementation) => new TokenContextHandle(implementation);
        public static IMemory ToHandle(this FZOSpec.IMemoryFZO implementation) => new MemoryHandle(implementation);
        public static IInput ToHandle(this FZOSpec.IInputFZO implementation) => new InputHandle(implementation);
        public static IMemory WithResolution(this IMemory state, ResObj resolution)
        {
            return resolution.Instructions.AccumulateInto(state, (prevState, instruction) => instruction.TransformMemory(prevState));
        }

    }
}