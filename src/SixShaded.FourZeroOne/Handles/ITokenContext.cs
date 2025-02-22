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
    public interface ITokenContext
    {
        public FZOSpec.IProcessorFZO.ITokenContext InternalValue { get; }
        public IMemory CurrentMemory { get; }
        public IInput Input { get; }
    }
    
}