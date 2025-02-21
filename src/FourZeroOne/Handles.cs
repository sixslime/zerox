
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using System;
using MorseCode.ITask;
using FourZeroOne.FZOSpec;
using SixShaded.NotRust;

#nullable enable
namespace FourZeroOne.Handles
{
    using Res = IResolution;
    using Addr = IMemoryAddress<IResolution>;
    using Rule = Rule.Unsafe.IRule<IResolution>;
    public interface IMemory
    {
        public FZOSpec.IMemoryFZO InternalValue { get; }
        public IEnumerable<ITiple<Addr, Res>> Objects { get; }
        public IEnumerable<Rule> Rules { get; }
        public IOption<R> GetObject<R>(IMemoryAddress<R> address) where R : class, Res;
        public IOption<Res> GetObjectUnsafe(Addr address);
        public IMemory WithRules(IEnumerable<Rule> rules);
        public IMemory WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) where R : class, Res;
        public IMemory WithObjectsUnsafe(IEnumerable<ITiple<Addr, Res>> insertions);
        public IMemory WithClearedAddresses(IEnumerable<Addr> removals);
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
        public ITask<int[]> ReadSelection(IHasElements<Res> pool, int count);
    }
    public class MemoryHandle(FZOSpec.IMemoryFZO implementation) : IMemory
    {
        private readonly FZOSpec.IMemoryFZO _implementation = implementation;
        IMemoryFZO IMemory.InternalValue => _implementation;
        IEnumerable<ITiple<Addr, Res>> IMemory.Objects => _implementation.Objects;
        IEnumerable<Rule> IMemory.Rules => _implementation.Rules;
        IOption<R> IMemory.GetObject<R>(IMemoryAddress<R> address) => _implementation.GetObject(address);
        IOption<Res> IMemory.GetObjectUnsafe(Addr address) => _implementation.GetObject((IMemoryAddress<Res>)address);
        IMemory IMemory.WithClearedAddresses(IEnumerable<Addr> removals) => _implementation.WithClearedAddresses(removals).ToHandle();
        IMemory IMemory.WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) => _implementation.WithObjects(insertions).ToHandle();
        IMemory IMemory.WithObjectsUnsafe(IEnumerable<ITiple<Addr, Res>> insertions) => _implementation.WithObjects(insertions.Map(x => ((IMemoryAddress<Res>)x.A, (Res)x.B).Tiple())).ToHandle();
        IMemory IMemory.WithRules(IEnumerable<Rule> rules) => _implementation.WithRules(rules).ToHandle();
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

        ITask<int[]> IInput.ReadSelection(IHasElements<Res> pool, int count) => _implementation.GetSelection(pool, count);
    }
    public static class Extensions
    {
        public static ITokenContext ToHandle(this FZOSpec.IProcessorFZO.ITokenContext implementation) => new TokenContextHandle(implementation);
        public static IMemory ToHandle(this FZOSpec.IMemoryFZO implementation) => new MemoryHandle(implementation);
        public static IInput ToHandle(this FZOSpec.IInputFZO implementation) => new InputHandle(implementation);
        public static IMemory WithResolution(this IMemory state, Res resolution)
        {
            return resolution.Instructions.AccumulateInto(state, (prevState, instruction) => instruction.TransformMemory(prevState));
        }

    }
}