using MorseCode.ITask;
using SixShaded.FourZeroOne;
using SixShaded.FourZeroOne.Rule.Defined.Unsafe;
using SixShaded.NotRust;
using SixShaded.SixLib.GFunc;
#nullable enable
namespace SixShaded.FourZeroOne.FZOSpec
{
    using Token;
    using Res = Resolution.Res;
    using Rule = IRule<Resolution.Res>;
    using ResOpt = IOption<Resolution.Res>;
    using Token = IToken<Resolution.Res>;
    using Addr = Resolution.IMemoryAddress<Resolution.Res>;
    using Resolution;
    using Resolution.Unsafe;
    using Rule;
    using System.Diagnostics.CodeAnalysis;
    using SixShaded.NotRust;
    public record FZOSource
    {
        public required Token Program { get; init; }
        public required IMemoryFZO InitialMemory { get; init; }
    }
}