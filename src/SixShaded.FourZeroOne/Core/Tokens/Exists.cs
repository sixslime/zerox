#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens
{
    public sealed record Exists : Function<ResObj, ro.Bool>
    {
        public Exists(IToken<ResObj> obj) : base(obj) { }
        protected override ITask<IOption<ro.Bool>> Evaluate(ITokenContext _, IOption<ResObj> obj)
        {
            return new ro.Bool() { IsTrue = obj.IsSome() }.AsSome().ToCompletedITask();
        }
    }
}