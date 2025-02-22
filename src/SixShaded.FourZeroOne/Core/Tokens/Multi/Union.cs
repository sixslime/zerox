namespace SixShaded.FourZeroOne.Core.Tokens.Multi;

public sealed record Union<R> : Token.Defined.PureCombiner<IMulti<R>, Resolutions.Multi<R>> where R : class, Res
{
    public Union(IEnumerable<IToken<IMulti<R>>> elements) : base(elements) { }
    public Union(params IToken<IMulti<R>>[] elements) : base(elements) { }
    protected override Resolutions.Multi<R> EvaluatePure(IEnumerable<IMulti<R>> inputs) => new() { Values = inputs.Map(x => x.Elements).Flatten().ToPSequence() };

    protected override IOption<string> CustomToString()
    {
        List<IToken<IMulti<R>>> argList = [.. Args];
        return $"[{string.Join(", ", Args.Map(x => x.ToString()))}]".AsSome();
    }
}