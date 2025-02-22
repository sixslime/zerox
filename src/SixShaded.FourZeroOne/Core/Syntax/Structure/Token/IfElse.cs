namespace SixShaded.FourZeroOne.Core.Syntax.Structure.Token;

public sealed record IfElse<R> where R : class, Res
{
    public required IToken<R> Then { get; init; }
    public required IToken<R> Else { get; init; }
}