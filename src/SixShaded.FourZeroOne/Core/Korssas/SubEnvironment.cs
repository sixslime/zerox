namespace SixShaded.FourZeroOne.Core.Korssas;

public record SubEnvironment<ROut> : Korssa.Defined.PureFunction<IMulti<Rog>, ROut, ROut>
    where ROut : class, Rog
{
    public SubEnvironment(IKorssa<IMulti<Rog>> envModifiers, IKorssa<ROut> evalKorssa) : base(envModifiers, evalKorssa) { }
    protected override ROut EvaluatePure(IMulti<Rog> _, ROut in2) => in2;
    protected override IOption<string> CustomToString() => $"let {Arg1} in {{{Arg2}}}".AsSome();
}