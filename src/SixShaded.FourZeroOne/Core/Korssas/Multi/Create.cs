namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

public sealed record Create<R> : Korssa.Defined.PureCombiner<R, Roggis.Multi<R>>
    where R : class, Rog
{
    public Create(params IKorssa<R>[] elements) : base(elements)
    { }

    public Create(IEnumerable<IKorssa<R>> elements) : base(elements);

    protected override Roggis.Multi<R> EvaluatePure(IEnumerable<R> inputs) =>
        new()
        {
            Values = inputs.ToPSequence(),
        };

    protected override IOption<string> CustomToString() => $"[{string.Join(", ", Args.Map(x => x.ToString()))}]".AsSome();
}