namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record Concat<R> : Korssa.Defined.Function<IMulti<R>, IMulti<R>, Multi<R>>
    where R : class, Rog
{
    public Concat(IKorssa<IMulti<R>> a, IKorssa<IMulti<R>> b) : base(a, b)
    { }

    protected override ITask<IOption<Multi<R>>> Evaluate(IKorssaContext _, IOption<IMulti<R>> aOpt, IOption<IMulti<R>> bOpt) =>
        new Multi<R>()
            {
                Values =
                    Iter.Over(aOpt, bOpt)
                        .Map(x => x.RemapAs(y => y.Elements).Or([]))
                        .Flatten()
                        .ToPSequence(),
            }.AsSome()
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{Arg1}++{Arg2}".AsSome();
}