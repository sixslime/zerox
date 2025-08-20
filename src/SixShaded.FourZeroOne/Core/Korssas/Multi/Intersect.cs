namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;
using Roggi.Unsafe;

public sealed record Intersect<R> : Korssa.Defined.PureFunction<IMulti<R>, IMulti<R>, Multi<R>>
    where R : class, Rog
{
    public Intersect(IKorssa<IMulti<R>> a, IKorssa<IMulti<R>> b) : base(a, b)
    { }

    protected override Multi<R> EvaluatePure(IMulti<R> a, IMulti<R> b) =>
        (a is IEfficientMulti<R> aEff && b is IEfficientMulti<R> bEff)
            ? aEff.Intersect(bEff).ToMulti()
            : new(a.Elements.Filtered().Intersect(b.Elements.Filtered()).Map(x => x.AsSome()));
    protected override IOption<string> CustomToString() => $"{Arg1} \u2229 {Arg2}".AsSome();
}