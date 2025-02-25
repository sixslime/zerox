namespace SixShaded.FourZeroOne.Core.Tokens.IO.Select;

using Resolutions;

public sealed record Multiple<R> : Token.Defined.Function<IMulti<R>, Number, Multi<R>> where R : class, Res
{
    public Multiple(IToken<IMulti<R>> from, IToken<Number> count) : base(from, count) { }

    protected override async ITask<IOption<Multi<R>>> Evaluate(ITokenContext runtime, IOption<IMulti<R>> fromOpt, IOption<Number> countOpt) =>
        fromOpt.Check(out var from) && countOpt.Check(out var count)
            ? new Multi<R>
                {
                    Values =
                        (await runtime.Input.ReadSelection(from, count.Value))
                        .Map(i => from.At(i).Expect($"Got invalid index '{i}', expected 0..{from.Count - 1}")).ToPSequence(),
                }
                .AsSome()
            : new None<Multi<R>>();

    protected override IOption<string> CustomToString() => $"SelectMulti({Arg1}, {Arg2})".AsSome();
}