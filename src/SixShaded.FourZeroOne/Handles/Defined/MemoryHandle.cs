using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixShaded.FourZeroOne.Handles.Defined
{
    using Rule = Rule.Unsafe.IRule<Res>;
    internal class MemoryHandle(FZOSpec.IMemoryFZO implementation) : IMemory
    {
        private readonly FZOSpec.IMemoryFZO _implementation = implementation;
        FZOSpec.IMemoryFZO IMemory.InternalValue => _implementation;
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
