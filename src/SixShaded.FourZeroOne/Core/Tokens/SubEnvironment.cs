namespace SixShaded.FourZeroOne.Core.Tokens;

public record SubEnvironment<ROut> : Token.Defined.PureFunction<IMulti<Res>, ROut, ROut>
    where ROut : class, Res
{
    public SubEnvironment(IToken<IMulti<Res>> envModifiers, IToken<ROut> evalToken) : base(envModifiers, evalToken) { }
    protected override ROut EvaluatePure(IMulti<Res> _, ROut in2) => in2;
    protected override IOption<string> CustomToString() => $"let {Arg1} in {{{Arg2}}}".AsSome();
}