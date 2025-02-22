#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Multi
{
    public sealed record Yield<R> : PureFunction<R, r.Multi<R>> where R : Res
    {
        public Yield(IToken<R> value) : base(value) { }
        protected override r.Multi<R> EvaluatePure(R in1)
        {
            return new() { Values = in1.Yield().ToPSequence() };
        }
        protected override IOption<string> CustomToString() => $"^{Arg1}".AsSome();
    }
}