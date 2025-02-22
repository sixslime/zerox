#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens
{
    public record SubEnvironment<ROut> : PureFunction<ResObj, ROut, ROut>
        where ROut : class, ResObj
    {
        public SubEnvironment(IToken<ResObj> envModifiers, IToken<ROut> evalToken) : base(envModifiers, evalToken) { }
        protected override ROut EvaluatePure(ResObj _, ROut in2)
        {
            return in2;
        }
        protected override IOption<string> CustomToString() => $"let {Arg1} in {{{Arg2}}}".AsSome();
    }
}