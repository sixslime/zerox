namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record GetSlice<R> : Korssa.Defined.PureFunction<IMulti<R>, NumRange, Multi<R>>
    where R : class, Rog
{
    public GetSlice(IKorssa<IMulti<R>> from, IKorssa<NumRange> index) : base(from, index)
    { }

    protected override Multi<R> EvaluatePure(IMulti<R> in1, NumRange in2) =>
        new()
        {
            Values =
                in2.Numbers.FilterMap(i => in1.At(i.Value-1)).ToPSequence()
        };
    protected override IOption<string> CustomToString() => $"{Arg1}[{Arg2}]".AsSome();
}