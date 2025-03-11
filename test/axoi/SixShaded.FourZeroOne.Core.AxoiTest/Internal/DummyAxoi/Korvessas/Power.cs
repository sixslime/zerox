namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Korvessas;

using Korssa;
using Korvessa.Defined;
using Korvessa;
using Roggi;
using Roggis;
using Roveggitus;
using Core = Syntax.Core;
using SixShaded.FourZeroOne.Roveggi;

internal static class Power
{
    public static Korvessa<IRoveggi<PowerExpr>, Number> Construct(IKorssa<IRoveggi<PowerExpr>> powerve) =>
        new(powerve)
        {
            Du = new(TestAxoi.Du, "power"),
            Definition =
                Core.kMetaFunction<IRoveggi<PowerExpr>, Number>([],
                    self =>
                        Core.kMetaFunctionRecursive<Number, Number, Number, Number>([],
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
                                A = self.kRef().kGetRovi(PowerExpr.NUM),
                                B = self.kRef().kGetRovi(PowerExpr.POWER),
                                C = self.kRef().kGetRovi(PowerExpr.NUM),
                            }))
        };

    public static Korvessa<IRoveggi<PowerExpr>, Number> kTESTPower(this IKorssa<IRoveggi<PowerExpr>> powerve) => Construct(powerve);
}