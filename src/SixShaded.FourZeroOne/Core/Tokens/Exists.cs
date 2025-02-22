#nullable enable
using FourZeroOne;

namespace SixShaded.FourZeroOne.Core.Tokens
{
    public sealed record Exists : Function<Res, ro.Bool>
    {
        public Exists(IToken<Res> obj) : base(obj) { }
        protected override ITask<IOption<ro.Bool>> Evaluate(ITokenContext _, IOption<Res> obj)
        {
            return new ro.Bool() { IsTrue = obj.IsSome() }.AsSome().ToCompletedITask();
        }
    }
}