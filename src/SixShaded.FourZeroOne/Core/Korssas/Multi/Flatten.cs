namespace SixShaded.FourZeroOne.Core.Korssas.Multi;

using Roggis;

public record Flatten<R>(IKorssa<IMulti<IMulti<R>>> multi) : Korssa.Defined.Function<IMulti<IMulti<R>>, Multi<R>>(multi)
    where R : class, Rog
{
    protected override ITask<IOption<Multi<R>>> Evaluate(IKorssaContext _, IOption<IMulti<IMulti<R>>> in1) =>
        in1.RemapAs(
            arr =>
                arr.Elements.All(x => x.IsSome())
                    .ToOptionLazy(() => new Multi<R>(arr.Elements.Map(x => x.Unwrap().Elements).Flatten())))
            .Press()
            .ToCompletedITask();

    protected override IOption<string> CustomToString() => $"{Arg1}__".AsSome();
}