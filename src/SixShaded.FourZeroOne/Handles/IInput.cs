
using FourZeroOne.Resolution;
using FourZeroOne.Resolution.Unsafe;
using FourZeroOne.Rule;
using System;
using MorseCode.ITask;
using FourZeroOne.FZOSpec;
using SixShaded.NotRust;
using SixShaded.FourZeroOne;

#nullable enable
namespace SixShaded.FourZeroOne.Handles
{
    using Res = IResolution;
    using Addr = IMemoryAddress<IResolution>;
    using Rule = Rule.Unsafe.IRule<IResolution>;
    public interface IInput
    {
        public FZOSpec.IInputFZO InternalValue { get; }
        public ITask<int[]> ReadSelection(IHasElements<Res> pool, int count);
    }
    
    public class InputHandle(FZOSpec.IInputFZO implementation) : IInput
    {
        private readonly FZOSpec.IInputFZO _implementation = implementation;

        IInputFZO IInput.InternalValue => _implementation;

        ITask<int[]> IInput.ReadSelection(IHasElements<Res> pool, int count) => _implementation.GetSelection(pool, count);
    }
}