#nullable enable
namespace SixShaded.FourZeroOne.Core.Tokens.Multi
{
    public sealed record Yield<R> : Token.Defined.PureFunction<R, Resolutions.Multi<R>> where R : class, Res
    {
        public Yield(IToken<R> value) : base(value) { }
        protected override Resolutions.Multi<R> EvaluatePure(R in1)
        {
            return new() { Values = in1.Yield().ToPSequence() };
        }
        protected override IOption<string> CustomToString() => $"^{Arg1}".AsSome();
    }
}