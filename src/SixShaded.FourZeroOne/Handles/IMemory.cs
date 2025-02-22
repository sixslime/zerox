using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using System;
using MorseCode.ITask;
using FourZeroOne.FZOSpec;
using SixShaded.NotRust;
using SixShaded.FourZeroOne;
using SixShaded.FourZeroOne.Rule.Defined.Unsafe;
using SixShaded.FourZeroOne.Resolution;

#nullable enable
namespace SixShaded.FourZeroOne.Handles
{
    using Res = Res;
    using Addr = IMemoryAddress<Res>;
    using Rule = IRule<Res>;
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
    
    internal class MemoryHandle(FZOSpec.IMemoryFZO implementation) : IMemory
    {
        private readonly FZOSpec.IMemoryFZO _implementation = implementation;
        IMemoryFZO IMemory.InternalValue => _implementation;
        IEnumerable<ITiple<Addr, Res>> IMemory.Objects => _implementation.Objects;
        IEnumerable<Rule> IMemory.Rules => _implementation.Rules;
        IOption<R> IMemory.GetObject<R>(IMemoryAddress<R> address) => _implementation.GetObject(address);
        IOption<Res> IMemory.GetObjectUnsafe(Addr address) => _implementation.GetObject(address);
        IMemory IMemory.WithClearedAddresses(IEnumerable<Addr> removals) => _implementation.WithClearedAddresses(removals).ToHandle();
        IMemory IMemory.WithObjects<R>(IEnumerable<ITiple<IMemoryAddress<R>, R>> insertions) => _implementation.WithObjects(insertions).ToHandle();
        IMemory IMemory.WithObjectsUnsafe(IEnumerable<ITiple<Addr, Res>> insertions) => _implementation.WithObjects(insertions.Map(x => ((Addr)x.A, (Res)x.B).Tiple())).ToHandle();
        IMemory IMemory.WithRules(IEnumerable<Rule> rules) => _implementation.WithRules(rules).ToHandle();
    }
}