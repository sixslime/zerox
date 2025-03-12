namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public record Flatten<R>(IKorssa<IMulti<IMulti<R>>> multi) : Korssa.Defined.PureFunction<IMulti<IMulti<R>>, Multi<R>>(multi)
    where R : class, Rog
{
    protected override Multi<R> EvaluatePure(IMulti<IMulti<R>> in1) =>
        new()
        {
            Values = in1.Elements.Map(x => x.Elements).Flatten().ToPSequence(),
        };
}