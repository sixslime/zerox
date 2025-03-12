namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record Intersection<R> : Korssa.Defined.PureFunction<IMulti<IMulti<R>>, Multi<R>>
    where R : class, Rog
{
    public Intersection(IKorssa<IMulti<IMulti<R>>> sets) : base(sets)
    { }

    protected override Multi<R> EvaluatePure(IMulti<IMulti<R>> inputs) =>
        new()
        {
            Values =
                inputs.Elements.Map(x => x.Elements)
                    .Accumulate((a, b) => a.Intersect(b))
                    .Or([])
                    .ToPSequence(),
        };

    protected override IOption<string> CustomToString() => $"I({Arg1})".AsSome();
}