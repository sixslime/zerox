#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens
{
    public record MetaExecuted<R> : PureFunction<R, R>
        where R : Res
    {
        public MetaExecuted(IToken<R> function) : base(function) { }
        protected override R EvaluatePure(R in1) => in1;
        protected override IOption<string> CustomToString() => $"|>{Arg1}".AsSome();
    }
}