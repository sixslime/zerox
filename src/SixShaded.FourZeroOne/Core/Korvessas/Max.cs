namespace SixShaded.FourZeroOne.Core.Korvessas;

using Korvessa.Defined;
using Syntax;
using Roggis;
public static class Max
{
    public static Korvessa<Number, Number, Number> Construct(IKorssa<Number> a, IKorssa<Number> b) =>
        new(a, b)
        {
            Du = Axoi.Korvedu("Max"),
            Definition =
                (_, iA, iB) =>
                    iA.kRef()
                        .kIsGreaterThan(iB.kRef())
                        .kIfTrue<Number>(
                        new()
                        {
                            Then = iA.kRef(),
                            Else = iB.kRef()
                        })

        };
}