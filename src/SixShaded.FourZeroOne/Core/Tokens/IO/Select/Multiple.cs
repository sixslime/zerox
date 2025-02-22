#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.IO.Select
{
    public sealed record Multiple<R> : Function<IMulti<R>, ro.Number, r.Multi<R>> where R : class, ResObj
    {
        public Multiple(IToken<IMulti<R>> from, IToken<ro.Number> count) : base(from, count) { }

        protected override async ITask<IOption<r.Multi<R>>> Evaluate(ITokenContext runtime, IOption<IMulti<R>> fromOpt, IOption<ro.Number> countOpt)
        {
            return fromOpt.Check(out var from) && countOpt.Check(out var count)
                ? new r.Multi<R>()
                {
                    Values =
                            (await runtime.Input.ReadSelection(from, count.Value))
                            .Map(i => from.At(i).Expect($"Got invalid index '{i}', expected 0..{from.Count - 1}")).ToPSequence()
                }
                .AsSome()
                : new None<r.Multi<R>>();
        }
        protected override IOption<string> CustomToString() => $"SelectMulti({Arg1}, {Arg2})".AsSome();
    }
}