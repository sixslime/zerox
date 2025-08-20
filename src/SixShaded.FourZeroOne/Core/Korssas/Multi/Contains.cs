namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record Contains<R> : Korssa.Defined.PureFunction<IMulti<R>, R, Bool>
    where R : class, Rog
{
    public Contains(IKorssa<IMulti<R>> multi, IKorssa<R> element) : base(multi, element)
    { }

    protected override Bool EvaluatePure(IMulti<R> in1, R in2) => in1.Elements.FilterMap(x => x).Contains(in2);
}