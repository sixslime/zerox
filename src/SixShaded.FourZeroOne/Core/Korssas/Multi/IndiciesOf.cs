namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record IndiciesOf<R> : Korssa.Defined.PureFunction<IMulti<R>, R, Multi<Number>>
    where R : class, Rog
{
    public IndiciesOf(IKorssa<IMulti<R>> multi, IKorssa<R> element) : base(multi, element)
    { }

    protected override Multi<Number> EvaluatePure(IMulti<R> in1, R in2) =>
        (in1 is Roggi.Unsafe.IEfficientMulti<R> eff)
            ? new(eff.IndexMap.At(in2).RemapAs(x => x.Elements.Map(y => ((Number)y).AsSome())).Or([]))
            : new(in1.Elements.Enumerate().FilterMap(x => x.value.Retain(y => y.Equals(in2)).RemapAs(_ => ((Number)x.index).AsSome())));
}