namespace SixShaded.FourZeroOne.Core.Syntax.Structure.Token;

public sealed record SubEnvironment<R> where R : class, Res
{
    public required Tok Environment { get; init; }
    public required IToken<R> Value { get; init; }
}