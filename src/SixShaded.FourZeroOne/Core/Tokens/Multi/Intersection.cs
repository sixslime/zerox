namespace SixShaded.FourZeroOne.Core.Tokens.Multi;

public sealed record Intersection<R> : Token.Defined.Combiner<IMulti<R>, Resolutions.Multi<R>> where R : class, Res
{
    public Intersection(IEnumerable<IToken<IMulti<R>>> sets) : base(sets) { }
    public Intersection(params IToken<IMulti<R>>[] sets) : base(sets) { }

    protected override ITask<IOption<Resolutions.Multi<R>>> Evaluate(ITokenContext _, IEnumerable<IOption<IMulti<R>>> inputs) =>
        new Resolutions.Multi<R>
        {
            Values = inputs
                    .Map(x => x.RemapAs(y => y.Elements).Or([])).Accumulate((a, b) => a.Intersect(b)).Or([]).ToPSequence(),
        }
            .AsSome().ToCompletedITask();

    protected override IOption<string> CustomToString()
    {
        List<IToken<IMulti<R>>> argList = [.. Args];
        return $"{argList[0]}{argList[1..].AccumulateInto("", (msg, v) => $"{msg}I{v}")}".AsSome();
    }
}