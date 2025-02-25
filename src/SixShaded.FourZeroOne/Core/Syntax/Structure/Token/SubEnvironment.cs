namespace SixShaded.FourZeroOne.Core.Syntax.Structure.Token;

public sealed record SubEnvironment<R> where R : class, Res
{
    public required List<Tok> Environment { get; init; }
    public required IToken<R> Value { get; init; }
}