namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Korvessas;

using Korssa;
using Korvessa.Defined;
using Korvessa;
using Roggi;
using Roggis;
using Roveggitus;
using Core = Syntax.Core;
using Roveggi;

internal static class Power
{
    public static Korvessa<IRoveggi<uPowerExpr>, Number> Construct(IKorssa<IRoveggi<uPowerExpr>> powerve) =>
        new(powerve)
        {
            Du = new(TestAxoi.Du, "power"),
            Definition =
                Core.kMetaFunction<IRoveggi<uPowerExpr>, Number>(
                [],
                self =>
                    Core.kMetaFunctionRecursive<Number, Number, Number, Number>(
                        [],
                        (recurse, acc, i, num) =>
                            i.kRef()
                                .kIsGreaterThan(1.kFixed())
                                .kIfTrue<Number>(
                                new()
                                {
                                    Then =
                                        recurse.kRef()
                                            .kExecuteWith(
                                            new()
                                            {
                                                A = acc.kRef().kMultiply(num.kRef()),
                                                B = i.kRef().kSubtract(1.kFixed()),
                                                C = num.kRef(),
                                            }),
                                    Else = acc.kRef(),
                                }))
                        .kExecuteWith(
                        new()
                        {
                            A = self.kRef().kGetRovi(uPowerExpr.NUM),
                            B = self.kRef().kGetRovi(uPowerExpr.POWER),
                            C = self.kRef().kGetRovi(uPowerExpr.NUM),
                        })),
        };

    public static Korvessa<IRoveggi<uPowerExpr>, Number> kTESTPower(this IKorssa<IRoveggi<uPowerExpr>> powerve) => Construct(powerve);
}