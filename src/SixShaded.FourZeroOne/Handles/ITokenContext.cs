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
    using Res = IResolution;
    using Addr = IMemoryAddress<IResolution>;
    using Rule = IRule<IResolution>;
    public interface ITokenContext
    {
        public FZOSpec.IProcessorFZO.ITokenContext InternalValue { get; }
        public IMemory CurrentMemory { get; }
        public IInput Input { get; }
    }
    public class TokenContextHandle(FZOSpec.IProcessorFZO.ITokenContext implementation) : ITokenContext
    {
        private readonly FZOSpec.IProcessorFZO.ITokenContext _implementation = implementation;
        IMemory ITokenContext.CurrentMemory => _implementation.CurrentMemory.ToHandle();
        IInput ITokenContext.Input => _implementation.Input.ToHandle();

        IProcessorFZO.ITokenContext ITokenContext.InternalValue => _implementation;
    }
}