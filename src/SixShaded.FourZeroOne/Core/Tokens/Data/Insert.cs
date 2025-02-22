#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Data
{
    public sealed record Insert<R> : PureFunction<IMemoryObject<R>, R, r.Instructions.Assign<R>>
        where R : class, ResObj
    {
        public Insert(IToken<IMemoryObject<R>> address, IToken<R> obj) : base(address, obj) { }
        protected override r.Instructions.Assign<R> EvaluatePure(IMemoryObject<R> in1, R in2)
        {
            return new() { Address = in1, Subject = in2 };
        }
        protected override IOption<string> CustomToString() => $"{Arg1} <== {Arg2}".AsSome();
    }
}