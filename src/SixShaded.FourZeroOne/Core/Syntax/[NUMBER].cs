namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class KorssaSyntax
{
    public static Korssas.Number.Add kAdd(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);
    public static Korssas.Number.Subtract kSubtract(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);
    public static Korssas.Number.Multiply kMultiply(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);
    public static Korssas.Number.Divide kDivide(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);
    public static Korssas.Number.Modulo kModulo(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);
    public static Korvessas.Min kAtMost(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);
    public static Korvessas.Max kAtLeast(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);
    public static Korvessas.Clamp kClamp(this IKorssa<Number> val, IKorssa<NumRange> range) => new(val, range);
    public static Korssas.Number.GreaterThan kIsGreaterThan(this IKorssa<Number> a, IKorssa<Number> b) => new(a, b);
}