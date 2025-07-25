namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class MapWithIndex<RIn, ROut>
    where RIn : class, Rog
    where ROut : class, Rog
{
    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, Number, ROut>, Multi<ROut>> Construct(IKorssa<IMulti<RIn>> multi, IKorssa<MetaFunction<RIn, Number, ROut>> mapFunction) =>
        new(multi, mapFunction)
        {
            Du = Axoi.Korvedu("MapWithIndex"),
            Definition =
                (_, iMulti, iMapFunction) =>
                    Core.kMetaFunctionRecursive<Number, Multi<ROut>>(
                        [],
                        (iRecurse, iIndex) =>
                            iIndex.kRef()
                                .kIsGreaterThan(iMulti.kRef().kCount())
                                .kIfTrue<Multi<ROut>>(
                                new()
                                {
                                    Then = Core.kMulti<ROut>(),
                                    Else =
                                        iMapFunction.kRef()
                                            .kExecuteWith(
                                            new()
                                            {
                                                A = iMulti.kRef().kGetIndex(iIndex.kRef()),
                                                B = iIndex.kRef()
                                            })
                                            .kYield()
                                            .kConcat(
                                            iRecurse.kRef()
                                                .kExecuteWith(
                                                new()
                                                {
                                                    A = iIndex.kRef().kAdd(1.kFixed()),
                                                }))
                                }))
                        .kExecuteWith(
                        new()
                        {
                            A = 1.kFixed(),
                        }),
        };
}