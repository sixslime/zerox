namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

public sealed record Exclusion<R> : Korssa.Defined.Function<IMulti<R>, IMulti<R>, Roggis.Multi<R>>
    where R : class, Rog
{
    public Exclusion(IKorssa<IMulti<R>> from, IKorssa<IMulti<R>> exclude) : base(from, exclude)
    { }

    protected override ITask<IOption<Roggis.Multi<R>>> Evaluate(IKorssaContext _, IOption<IMulti<R>> in1, IOption<IMulti<R>> in2) =>
        in1.RemapAs(
            from =>
                new Roggis.Multi<R>
                {
                    Values = in2.RemapAs(sub => from.Elements.Except(sub.Elements)).Or([]).ToPSequence(),
                })
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{Arg1} - {Arg2}".AsSome();
}