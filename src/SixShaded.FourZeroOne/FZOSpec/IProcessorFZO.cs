using MorseCode.ITask;
using SixShaded.FourZeroOne;
using SixShaded.FourZeroOne.Rule.Defined.Unsafe;
using SixShaded.NotRust;
using SixShaded.SixLib.GFunc;
#nullable enable
namespace SixShaded.FourZeroOne.FZOSpec
{
    using Token;
    using Res = Resolution.IResolution;
    using Rule = IRule<Resolution.IResolution>;
    using ResOpt = IOption<Resolution.IResolution>;
    using Token = IToken<Resolution.IResolution>;
    using Addr = Resolution.IMemoryAddress<Resolution.IResolution>;
    using Resolution;
    using Resolution.Unsafe;
    using Rule;
    using System.Diagnostics.CodeAnalysis;
    using SixShaded.NotRust;

    public interface IProcessorFZO
    {
        public ITask<IResult<EProcessorStep, EProcessorHalt>> GetNextStep(IStateFZO state, IInputFZO input);
        public interface ITokenContext
        {
            public IMemoryFZO CurrentMemory { get; }
            public IInputFZO Input { get; }
        }
    }
}