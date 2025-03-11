namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

public sealed record Union<R> : Korssa.Defined.PureCombiner<IMulti<R>, Roggis.Multi<R>>
    where R : class, Rog
{
    public Union(IEnumerable<IKorssa<IMulti<R>>> elements) : base(elements)
    { }

    public Union(params IKorssa<IMulti<R>>[] elements) : base(elements)
    { }

    protected override Roggis.Multi<R> EvaluatePure(IEnumerable<IMulti<R>> inputs) =>
        new()
        {
            Values = inputs.Map(x => x.Elements).Flatten().ToPSequence(),
        };

    protected override IOption<string> CustomToString()
    {
        List<IKorssa<IMulti<R>>> argList = [.. Args];
        return $"[{string.Join(", ", Args.Map(x => x.ToString()))}]".AsSome();
    }
}