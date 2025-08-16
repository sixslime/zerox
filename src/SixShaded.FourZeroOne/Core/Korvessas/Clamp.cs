namespace SixShaded.FourZeroOne.Core.Korvessas;

using Korvessa.Defined;
using Syntax;
using Roggis;
public static class Clamp
{
    public static Korvessa<Number, NumRange, Number> Construct(IKorssa<Number> val, IKorssa<NumRange> range) =>
        new(val, range)
        {
            Du = Axoi.Korvedu("Clamp"),
            Definition =
                (_, iVal, iRange) =>
                    iVal.kRef()
                        .kClampMax(iRange.kRef().kEnd())
                        .kClampMin(iRange.kRef().kStart())
        };
}