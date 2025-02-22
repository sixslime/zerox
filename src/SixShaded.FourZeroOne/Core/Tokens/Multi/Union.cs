#nullable enable
namespace FourZeroOne.Core.Tokens.Multi
{
    public sealed record Union<R> : PureCombiner<IMulti<R>, r.Multi<R>> where R : class, ResObj
    {
        public Union(IEnumerable<IToken<IMulti<R>>> elements) : base(elements) { }
        public Union(params IToken<IMulti<R>>[] elements) : base(elements) { }
        protected override r.Multi<R> EvaluatePure(IEnumerable<IMulti<R>> inputs)
        {
            return new() { Values = inputs.Map(x => x.Elements).Flatten().ToPSequence() };
        }
        protected override IOption<string> CustomToString()
        {
            List<IToken<IMulti<R>>> argList = [.. Args];
            return $"[{string.Join(", ", Args.Map(x => x.ToString()))}]".AsSome();
        }
    }
}