#nullable enable
namespace FourZeroOne.Core.Tokens.Component
{
    public sealed record DoMerge<C> : Function<ICompositionOf<C>, ICompositionOf<r.MergeSpec<C>>, ICompositionOf<C>> where C : ICompositionType
    {
        public DoMerge(IToken<ICompositionOf<C>> in1, IToken<ICompositionOf<r.MergeSpec<C>>> in2) : base(in1, in2) { }

        protected override ITask<IOption<ICompositionOf<C>>> Evaluate(ITokenContext runtime, IOption<ICompositionOf<C>> in1, IOption<ICompositionOf<r.MergeSpec<C>>> in2)
        {
            return (in1.Check(out var subject) & in2.Check(out var merger)).ToOptionLazy(() =>
                subject.WithComponentsUnsafe(
                        merger.ComponentsUnsafe
                        .FilterMap(x => (x.A as r._Private.IMergeIdentifier<C>).NullToNone().RemapAs(y => (y.ForComponentUnsafe, x.B).Tiple()))))
                .ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"{Arg1}>>{Arg2}".AsSome();
    }
}