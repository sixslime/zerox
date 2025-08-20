namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public record Clean<R>(IKorssa<IMulti<R>> multi) : Korssa.Defined.PureFunction<IMulti<R>, Multi<R>>(multi)
    where R : class, Rog
{
    protected override Multi<R> EvaluatePure(IMulti<R> in1) =>
        new Multi<R>(in1.Elements.Where(x => x.IsSome()));
}