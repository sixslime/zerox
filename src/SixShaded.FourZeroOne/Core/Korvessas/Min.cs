namespace SixShaded.FourZeroOne.Core.Korvessas;

using Korvessa.Defined;
using Syntax;
using Roggis;
public static class Min
{
    public static Korvessa<Number, Number, Number> Construct(IKorssa<Number> a, IKorssa<Number> b) =>
        new(a, b)
        {
            Du = Axoi.Korvedu("Min"),
            Definition =
                (_, iA, iB) =>
                    iA.kRef()
                        .kIsGreaterThan(iB.kRef())
                        .kIfTrue<Number>(
                        new()
                        {
                            Then = iB.kRef(),
                            Else = iA.kRef()
                        })

        };
}