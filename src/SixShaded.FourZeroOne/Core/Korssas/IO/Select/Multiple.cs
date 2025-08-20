namespace SixShaded.FourZeroOne.Core.Korssas.IO.Select;

using Roggis;

public sealed record Multiple<R> : Korssa.Defined.Function<IMulti<R>, NumRange, Multi<R>>
    where R : class, Rog
{
    public Multiple(IKorssa<IMulti<R>> from, IKorssa<NumRange> count) : base(from, count)
    { }

    protected override async ITask<IOption<Multi<R>>> Evaluate(IKorssaContext runtime, IOption<IMulti<R>> fromOpt, IOption<NumRange> countOpt) =>
        (fromOpt.Check(out var multi) & countOpt.Check(out var count))
            ? await multi.Elements.FilterMap(x => x)
                .ToPSequence()
                .ExprAs(
                async from =>
                    (from.Count >= count.Start.Value && count.End.Value >= 0)
                        ? (count.End.Value > 0)
                            ? (await runtime.Input.ReadSelection(from, int.Max(count.Start.Value, 0), count.End.Value))
                            .ExprAs(
                            selections =>
                                selections.Length == selections.Distinct().Count()
                                    ? selections.Length >= count.Start.Value && selections.Length <= count.End.Value
                                        ? new Multi<R>(selections.Map(i => from.At(i).Expect(() => new InvalidOperationException($"Got invalid index '{i}', expected 0..{from.Count - 1}")).AsSome())).AsSome()
                                        : throw new InvalidOperationException($"Invalid amount of elements in selection [{string.Join(',', selections)}], expected {count.Start.Value}..{count.End.Value}, got {selections.Length}.")
                                    : throw new InvalidOperationException($"Duplicate(s) in selection [{string.Join(',', selections)}] (not allowed)"))
                            : new Multi<R>([]).AsSome()
                        : new None<Multi<R>>())
            : new None<Multi<R>>();

    protected override IOption<string> CustomToString() => $"SelectMulti({Arg1}, {Arg2})".AsSome();
}