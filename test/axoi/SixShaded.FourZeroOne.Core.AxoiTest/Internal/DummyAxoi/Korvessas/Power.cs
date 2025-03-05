namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Korvessas;

using Korssa;
using Korvessa.Defined;
using Korvessa;
using Roggi;
using Roggis;
using Roveggitus;
using Core = Syntax.Core;

internal static class Power
{
    public static Korvessa<IRoveggi<PowerExpr>, Number> Construct(IKorssa<IRoveggi<PowerExpr>> powerve) =>
        new(powerve)
        {
            Du = new(TestAxoi.Du, "power"),
            Definition =
                Core.tMetaFunction<IRoveggi<PowerExpr>, Number>(
                    self =>
                        Core.tMetaRecursiveFunction<Number, Number, Number, Number>(
                            (recurse, acc, i, num) =>
                                i.tRef()
                                    .tIsGreaterThan(1.kFixed())
                                    .kIfTrue<Number>(
                                    new()
                                    {
                                        Then =
                                            recurse.tRef()
                                                .tExecuteWith(
                                                new()
                                                {
                                                    A = acc.tRef().tMultiply(num.tRef()),
                                                    B = i.tRef().tSubtract(1.kFixed()),
                                                    C = num.tRef(),
                                                }),
                                        Else = acc.tRef(),
                                    }))
                            .tExecuteWith(
                            new()
                            {
                                A = self.tRef().kGetRovi(PowerExpr.NUM),
                                B = self.tRef().kGetRovi(PowerExpr.POWER),
                                C = self.tRef().kGetRovi(PowerExpr.NUM),
                            }))
                    .Roggi,
        };

    public static Korvessa<IRoveggi<PowerExpr>, Number> tTESTPower(this IKorssa<IRoveggi<PowerExpr>> powerve) => Construct(powerve);
}