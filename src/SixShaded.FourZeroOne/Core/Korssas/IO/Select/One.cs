namespace SixShaded.FourZeroOne.Core.Korssas.IO.Select;

public sealed record One<R> : Korssa.Defined.Function<IMulti<R>, R>
    where R : class, Rog
{
    public One(IKorssa<IMulti<R>> from) : base(from)
    { }

    protected override ITask<IOption<R>> Evaluate(IKorssaContext runtime, IOption<IMulti<R>> fromOpt) =>
        fromOpt.RemapAs(
            multi =>
                multi.Elements.FilterMap(x => x)
                    .ToPSequence()
                    .ExprAs(
                    from =>
                        (from.Count > 0)
                        .ToOptionLazy(
                        async () =>
                            (await runtime.Input.ReadSelection(from, 1))[0]
                            .ExprAs(i => from.At(i).Expect($"Got invalid index '{i}', expected 0..{from.Count - 1}"))
                            .AsSome())))
            .Press()
            .RemapAs(x => x.AsITask())
            .Or(new None<R>().ToCompletedITask());


    protected override IOption<string> CustomToString() => $"Select({Arg1})".AsSome();
}