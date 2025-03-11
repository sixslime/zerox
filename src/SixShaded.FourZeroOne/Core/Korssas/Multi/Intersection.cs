namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record Intersection<R> : Korssa.Defined.PureFunction<IMulti<IMulti<R>>, Roggis.Multi<R>>
    where R : class, Rog
{
    public Intersection(IKorssa<IMulti<IMulti<R>>> sets) : base(sets)
    { }

    protected override Multi<R> EvaluatePure(IMulti<IMulti<R>> inputs)
    {
        return new Multi<R>
        {
            Values =
                inputs.Elements.Map(x => x.Elements)
                    .Accumulate((a, b) => a.Intersect(b))
                    .Or([])
                    .ToPSequence()
        };
    }


    protected override IOption<string> CustomToString()
    {
        return $"I({Arg1})".AsSome();
    }
}