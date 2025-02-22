#nullable enable
namespace SixShaded.FourZeroOne.Core.Tokens
{
    public sealed record Nolla<R> : Token.Defined.Value<R> where R : class, Res
    {
        public Nolla() { }
        protected override ITask<IOption<R>> Evaluate(ITokenContext _) { return new None<R>().ToCompletedITask(); }
        protected override IOption<string> CustomToString() => "nolla".AsSome();
    }
}