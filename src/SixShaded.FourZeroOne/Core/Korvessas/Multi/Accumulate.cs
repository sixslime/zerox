namespace SixShaded.FourZeroOne.Core.Korvessas.Multi;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record Accumulate<RIn, ROut>(IKorssa<IMulti<RIn>> source, IKorssa<ROut> initialValue, IKorssa<MetaFunction<ROut, RIn, ROut>> function) : Korvessa<IMulti<RIn>, ROut, MetaFunction<ROut, RIn, ROut>, ROut>(source, initialValue, function)
    where RIn : class, Rog
    where ROut : class, Rog
{
    protected override RecursiveMetaDefinition<IMulti<RIn>, ROut, MetaFunction<ROut, RIn, ROut>, ROut> InternalDefinition() =>
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
                });
}