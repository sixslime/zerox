namespace SixShaded.FourZeroOne.Core.Tokens;

public sealed record Nolla<R> : Token.Defined.Value<R> where R : class, Res
{
    protected override ITask<IOption<R>> Evaluate(ITokenContext _) => new None<R>().ToCompletedITask();
    protected override IOption<string> CustomToString() => "nolla".AsSome();
}