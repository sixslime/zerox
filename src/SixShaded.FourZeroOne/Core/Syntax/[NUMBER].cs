namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;

public static partial class KorssaSyntax
{
    public static Korssas.Number.Add tAdd(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);

    public static Korssas.Number.Subtract tSubtract(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);

    public static Korssas.Number.Multiply tMultiply(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);

    public static Korssas.Number.GreaterThan tIsGreaterThan(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);
}