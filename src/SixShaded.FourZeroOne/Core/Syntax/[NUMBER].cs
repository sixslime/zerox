namespace SixShaded.FourZeroOne.Core.Syntax;

using Resolutions;

public static partial class TokenSyntax
{
    public static Tokens.Number.Add tAdd(this IToken<Number> a, IToken<Number> b) => new(a, b);

    public static Tokens.Number.Subtract tSubtract(this IToken<Number> a, IToken<Number> b) => new(a, b);

    public static Tokens.Number.Multiply tMultiply(this IToken<Number> a, IToken<Number> b) => new(a, b);

    public static Tokens.Number.GreaterThan tIsGreaterThan(this IToken<Number> a, IToken<Number> b) => new(a, b);
}