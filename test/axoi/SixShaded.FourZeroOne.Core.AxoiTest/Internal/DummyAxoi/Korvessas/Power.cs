namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Korvessas;

using Korssa;
using Korvessa.Defined;
using Korvessa;
using Roggi;
using Roggis;
using Rovetus;
using Core = Syntax.Core;
using Roveggi;

internal static class Power
{
    public static Korvessa<IRoveggi<uPowerExpr>, Number> Construct(IKorssa<IRoveggi<uPowerExpr>> powerve) =>
        new(powerve)
        {
            Du = new(TestAxoi.Du, "power"),
            Definition =
                (_, iSelf) =>
                    Core.kMetaFunctionRecursive<Number, Number, Number, Number>(
                        [],
                        (iRecurse, iAcc, iI, iNum) =>
                            iI.kRef()
                                .kIsGreaterThan(1.kFixed())
                                .kIfTrue<Number>(
                                new()
                                {
                                    Then =
                                        iRecurse.kRef()
                                            .kExecuteWith(
                                            new()
                                            {
                                                A = iAcc.kRef().kMultiply(iNum.kRef()),
                                                B = iI.kRef().kSubtract(1.kFixed()),
                                                C = iNum.kRef(),
                                            }),
                                    Else = iAcc.kRef(),
                                }))
                        .kExecuteWith(
                        new()
                        {
                            A = iSelf.kRef().kGetRovi(uPowerExpr.NUM),
                            B = iSelf.kRef().kGetRovi(uPowerExpr.POWER),
                            C = iSelf.kRef().kGetRovi(uPowerExpr.NUM),
                        }),
        };

    public static Korvessa<IRoveggi<uPowerExpr>, Number> kTESTPower(this IKorssa<IRoveggi<uPowerExpr>> powerve) => Construct(powerve);
}