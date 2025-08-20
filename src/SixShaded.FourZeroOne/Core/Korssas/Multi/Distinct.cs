namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record Distinct<R> : Korssa.Defined.PureFunction<IMulti<R>, Multi<R>>
    where R : class, Rog
{
    public Distinct(IKorssa<IMulti<R>> source) : base(source)
    { }

    protected override Multi<R> EvaluatePure(IMulti<R> in1) =>
        (in1 is Roggi.Unsafe.IEfficientMulti<R> eff)
            ? eff.Distinct().ToMulti()
            : new(in1.Elements.Filtered().Distinct().Map(x => x.AsSome()));
    protected override IOption<string> CustomToString() => $"Distinct({Arg1})".AsSome();
}