namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Accumulate<RIn, ROut>
    where RIn : class, Rog
    where ROut : class, Rog
{
    public static Korvessa<IMulti<RIn>, ROut, MetaFunction<ROut, RIn, ROut>, ROut> Construct(IKorssa<IMulti<RIn>> multi, IKorssa<ROut> initialValue, IKorssa<MetaFunction<ROut, RIn, ROut>> function) =>
        new(multi, initialValue, function)
        {
            Du = Axoi.Korvedu("Filter"),
            Definition =
                (_, iMulti, iInitial, iFunction) =>
                    Core.kMetaFunctionRecursive<ROut, Number, ROut>(
                        [],
                        (iRecurse, iAcc, iIndex) =>
                            iIndex.kRef()
                                .kIsGreaterThan(iMulti.kRef().kCount())
                                .kIfTrue<ROut>(
                                new()
                                {
                                    Then = iAcc.kRef(),
                                    Else =
                                        Core.kSubEnvironment<ROut>(
                                        new()
                                        {
                                            Environment =
                                            [
                                                iMulti.kRef()
                                                    .kGetIndex(iIndex.kRef())
                                                    .kAsVariable(out var iElement)
                                            ],
                                            Value =
                                                iRecurse.kRef()
                                                    .kExecuteWith(
                                                    new()
                                                    {
                                                        A =
                                                            iFunction.kRef()
                                                                .kExecuteWith(
                                                                new()
                                                                {
                                                                    A = iAcc.kRef(),
                                                                    B = iElement.kRef()
                                                                }),
                                                        B = iIndex.kRef().kAdd(1.kFixed())
                                                    })
                                        })
                                }))
                        .kExecuteWith(
                        new()
                        {
                            A = iInitial.kRef(),
                            B = 1.kFixed()
                        }),
        };
}