namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

public sealed record Intersection<R> : Korssa.Defined.Combiner<IMulti<R>, Roggis.Multi<R>>
    where R : class, Rog
{
    public Intersection(IEnumerable<IKorssa<IMulti<R>>> sets) : base(sets)
    { }

    public Intersection(params IKorssa<IMulti<R>>[] sets) : base(sets)
    { }

    protected override ITask<IOption<Roggis.Multi<R>>> Evaluate(IKorssaContext _, IEnumerable<IOption<IMulti<R>>> inputs) =>
        new Roggis.Multi<R>
            {
                Values =
                    inputs
                        .Map(x => x.RemapAs(y => y.Elements).Or([]))
                        .Accumulate((a, b) => a.Intersect(b))
                        .Or([])
                        .ToPSequence(),
            }
            .AsSome()
            .ToCompletedITask();

    protected override IOption<string> CustomToString()
    {
        List<IKorssa<IMulti<R>>> argList = [.. Args];
        return $"{argList[0]}{argList[1..].AccumulateInto("", (msg, v) => $"{msg}I{v}")}".AsSome();
    }
}