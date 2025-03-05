namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;

public static partial class KorssaSyntax
{
    public static Korssas.Number.Add kAdd(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);

    public static Korssas.Number.Subtract kSubtract(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);

    public static Korssas.Number.Multiply kMultiply(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);

    public static Korssas.Number.GreaterThan kIsGreaterThan(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);
}