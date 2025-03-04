namespace SixShaded.FourZeroOne.Core.Korssas.IO.Select;

using Roggis;

public sealed record Multiple<R> : Korssa.Defined.Function<IMulti<R>, Number, Multi<R>>
    where R : class, Rog
{
    public Multiple(IKorssa<IMulti<R>> from, IKorssa<Number> count) : base(from, count)
    { }

    protected override async ITask<IOption<Multi<R>>> Evaluate(IKorssaContext runtime, IOption<IMulti<R>> fromOpt, IOption<Number> countOpt) =>
        fromOpt.Check(out var from) && countOpt.Check(out var count) && count.Value <= from.Count
            ? (await runtime.Input.ReadSelection(from, count.Value))
            .ExprAs(
            selections =>
                selections.Length == selections.Distinct().Count()
                    ? new Multi<R>
                    {
                        Values =
                            selections.Map(i => from.At(i).Expect(() => new InvalidOperationException($"Got invalid index '{i}', expected 0..{from.Count - 1}")))
                                .ToPSequence(),
                    }.AsSome()
                    : throw new InvalidOperationException($"Duplicate(s) in selection [{string.Join(',', selections)}] (not allowed)"))
            : new None<Multi<R>>();

    protected override IOption<string> CustomToString() => $"SelectMulti({Arg1}, {Arg2})".AsSome();
}