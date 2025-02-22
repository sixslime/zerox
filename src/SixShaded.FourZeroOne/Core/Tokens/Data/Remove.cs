#nullable enable
namespace FourZeroOne.Core.Tokens.Data
{
    public sealed record Remove : PureFunction<IMemoryObject<ResObj>, r.Instructions.Redact>
    {
        public Remove(IToken<IMemoryObject<ResObj>> address) : base(address) { }
        protected override r.Instructions.Redact EvaluatePure(IMemoryObject<ResObj> in1)
        {
            return new() { Address = in1 };
        }
        protected override IOption<string> CustomToString() => $"{Arg1} <=X".AsSome();
    }
}