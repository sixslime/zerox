namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;

public static partial class KorssaSyntax
{
    public static Korssas.Range.Create kRangeForwardTo(this IKorssa<Number> start, IKorssa<Number> end) => new(start, end);
    public static Korssas.Range.Create kRangeBackwardTo(this IKorssa<Number> end, IKorssa<Number> start) => new(start, end);
    public static Korssas.Range.Get.Start kStart(this IKorssa<NumRange> range) => new(range);
    public static Korssas.Range.Get.End kEnd(this IKorssa<NumRange> range) => new(range);
}