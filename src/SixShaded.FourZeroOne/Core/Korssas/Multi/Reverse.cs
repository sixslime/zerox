namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record Reverse<R> : Korssa.Defined.PureFunction<IMulti<R>, Multi<R>>
    where R : class, Rog
{
    public Reverse(IKorssa<IMulti<R>> source) : base(source)
    { }

    protected override Multi<R> EvaluatePure(IMulti<R> in1) =>
        new()
        {
            Values = in1.Elements.Reverse().ToPSequence()
        };
    protected override IOption<string> CustomToString() => $"Reversed({Arg1})".AsSome();
}