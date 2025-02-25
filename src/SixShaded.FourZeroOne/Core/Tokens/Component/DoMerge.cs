namespace SixShaded.FourZeroOne.Core.Tokens.Component;

using Resolutions.Compositions;

public sealed record DoMerge<C> : Token.Defined.Function<ICompositionOf<C>, ICompositionOf<MergeSpec<C>>, ICompositionOf<C>> where C : ICompositionType
{
    public DoMerge(IToken<ICompositionOf<C>> in1, IToken<ICompositionOf<MergeSpec<C>>> in2) : base(in1, in2) { }

    protected override ITask<IOption<ICompositionOf<C>>> Evaluate(ITokenContext runtime, IOption<ICompositionOf<C>> in1, IOption<ICompositionOf<MergeSpec<C>>> in2) =>
        (in1.Check(out var subject) & in2.Check(out var merger)).ToOptionLazy(() =>
            subject.WithComponentsUnsafe(
                merger.ComponentsUnsafe
                    .FilterMap(x => (x.A as MergeSpec<C>.IMergeIdentifier).NullToNone().RemapAs(y => (y.ForComponentUnsafe, x.B).Tiple()))))
        .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{Arg1}>>{Arg2}".AsSome();
}