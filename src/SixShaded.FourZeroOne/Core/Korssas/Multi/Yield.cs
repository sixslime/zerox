namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public sealed record Yield<R> : Korssa.Defined.Function<R, Roggis.Multi<R>>
    where R : class, Rog
{
    public Yield(IKorssa<R> value) : base(value)
    { }

    protected override ITask<IOption<Multi<R>>> Evaluate(IKorssaContext _, IOption<R> in1) =>
        new Multi<R>
            {
                Values = in1.Yield().ToPSequence()
            }.AsSome()
            .ToCompletedITask();
    protected override IOption<string> CustomToString() => $"^{Arg1}".AsSome();
}