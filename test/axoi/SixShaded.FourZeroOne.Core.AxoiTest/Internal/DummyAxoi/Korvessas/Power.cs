namespace SixShaded.FourZeroOne.Core.AxoiTest.Internal.DummyAxoi.Korvessas;

using Korssa;
using Korvessa.Defined;
using Korvessa;
using Roggi;
using Roggis;
using Rovetus;
using Core = Syntax.Core;
using Roveggi;

internal record Power(IKorssa<IRoveggi<uPowerExpr>> powerve) : Korvessa<IRoveggi<uPowerExpr>, Number>(powerve)
{
    protected override RecursiveMetaDefinition<IRoveggi<uPowerExpr>, Number> InternalDefinition() =>
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
                });
}

internal static class PowerSyntax
{
    public static Power kTESTPower(this IKorssa<IRoveggi<uPowerExpr>> powerve) => new(powerve);
}