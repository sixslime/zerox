#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Multi
{
    public sealed record Count : Function<IMulti<ResObj>, ro.Number>
    {
        public Count(IToken<IMulti<ResObj>> of) : base(of) { }
        protected override ITask<IOption<ro.Number>> Evaluate(ITokenContext _, IOption<IMulti<ResObj>> in1)
        {
            return new ro.Number() { Value = in1.RemapAs(x => x.Count).Or(0) }.AsSome().ToCompletedITask();
        }
        protected override IOption<string> CustomToString() => $"{Arg1}.len".AsSome();
    }
}