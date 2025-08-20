namespace SixShaded.FourZeroOne.Core.Korvessas;

using Korvessa.Defined;
using Syntax;
using Roggis;
public static class SingleRange
{
    public static Korvessa<Number, NumRange> Construct(IKorssa<Number> number) =>
        new(number)
        {
            Du = Axoi.Korvedu("SingleRange"),
            Definition =
                (_, iNum) =>
                    iNum.kRef().kRangeTo(iNum.kRef())

        };
}