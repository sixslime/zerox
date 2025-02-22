#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens
{
    public record SubEnvironment<ROut> : PureFunction<Res, ROut, ROut>
        where ROut : class, Res
    {
        public SubEnvironment(IToken<Res> envModifiers, IToken<ROut> evalToken) : base(envModifiers, evalToken) { }
        protected override ROut EvaluatePure(Res _, ROut in2)
        {
            return in2;
        }
        protected override IOption<string> CustomToString() => $"let {Arg1} in {{{Arg2}}}".AsSome();
    }
}