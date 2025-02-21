
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
    internal static class HandlesSyntax
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