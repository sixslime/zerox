#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens.Multi
{
    public sealed record Contains<R> : Function<IMulti<R>, R, ro.Bool> where R : class, Res
    {
        public Contains(IToken<IMulti<R>> multi, IToken<R> element) : base(multi, element) { }
        protected override ITask<IOption<ro.Bool>> Evaluate(ITokenContext runtime, IOption<IMulti<R>> in1, IOption<R> in2)
        {
            return in2.RemapAs(item => new ro.Bool() { IsTrue = in1.RemapAs(arr => arr.Elements.Contains(item)).Or(false) }).ToCompletedITask();
        }
    }
}