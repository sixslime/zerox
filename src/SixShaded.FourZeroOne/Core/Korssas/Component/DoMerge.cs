namespace SixShaded.FourZeroOne.Core.Korssas.Component;

using Rovetus;

public sealed record DoMerge<C> : Korssa.Defined.Function<IRoveggi<C>, IRoveggi<MergeSpec<C>>, IRoveggi<C>> where C : IRovetu
{
    public DoMerge(IKorssa<IRoveggi<C>> in1, IKorssa<IRoveggi<MergeSpec<C>>> in2) : base(in1, in2) { }

    protected override ITask<IOption<IRoveggi<C>>> Evaluate(IKorssaContext runtime, IOption<IRoveggi<C>> in1, IOption<IRoveggi<MergeSpec<C>>> in2) =>
        (in1.Check(out var subject) & in2.Check(out var merger)).ToOptionLazy(() =>
            subject.WithComponentsUnsafe(
                merger.ComponentsUnsafe
                    .FilterMap(x => (x.A as MergeSpec<C>.IMergeRovu).NullToNone().RemapAs(y => (y.ForRovuUnsafe, x.B).Tiple()))))
        .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{Arg1}>>{Arg2}".AsSome();
}