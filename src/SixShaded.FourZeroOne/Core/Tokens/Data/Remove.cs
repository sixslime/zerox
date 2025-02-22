#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Data
{
    public sealed record Remove : PureFunction<IMemoryObject<Res>, r.Instructions.Redact>
    {
        public Remove(IToken<IMemoryObject<Res>> address) : base(address) { }
        protected override r.Instructions.Redact EvaluatePure(IMemoryObject<Res> in1)
        {
            return new() { Address = in1 };
        }
        protected override IOption<string> CustomToString() => $"{Arg1} <=X".AsSome();
    }
}