#nullable enable
namespace FourZeroOne.Core.Tokens.Multi
{
    public sealed record GetIndex<R> : Function<IMulti<R>, ro.Number, R> where R : class, ResObj
    {
        public GetIndex(IToken<IMulti<R>> from, IToken<ro.Number> index) : base(from, index) { }
        protected override ITask<IOption<R>> Evaluate(ITokenContext _, IOption<IMulti<R>> in1, IOption<ro.Number> in2)
        {
            var o = in1.Check(out var from) && in2.Check(out var index)
                ? from.At(index.Value - 1)
                : new None<R>();
            return o.ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"{Arg1}[{Arg2}]".AsSome();
    }
}