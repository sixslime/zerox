namespace SixShaded.FourZeroOne.Core.Korssas;

public record MetaExecuted<R> : Korssa.Defined.PureFunction<R, R>
    where R : class, Rog
{
    public MetaExecuted(IKorssa<R> function) : base(function) { }
    protected override R EvaluatePure(R in1) => in1;
    protected override IOption<string> CustomToString() => $"|>{Arg1}".AsSome();
}