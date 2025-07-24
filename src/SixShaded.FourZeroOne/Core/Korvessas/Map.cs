namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Map<RIn, ROut>
    where RIn : class, Rog
    where ROut : class, Rog
{
    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>> Construct(IKorssa<IMulti<RIn>> multi, IKorssa<MetaFunction<RIn, ROut>> mapFunction) =>
        new(multi, mapFunction)
        {
            Du = Axoi.Korvedu("Map"),
            Definition =
                Core.kMetaFunction<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>>(
                [],
                (iMulti, iMapFunction) =>
                    Core.kMetaFunctionRecursive<Number, Multi<ROut>>(
                        [],
                        (iRecurse, iIndex) =>
                            iIndex.kRef()
                                .kIsGreaterThan(iMulti.kRef().kCount())
                                .kIfTrue<Multi<ROut>>(
                                new()
                                {
                                    Then = Core.kNollaFor<Multi<ROut>>(),
                                    Else =
                                        iMapFunction.kRef()
                                            .kExecuteWith(
                                            new()
                                            {
                                                A = iMulti.kRef().kGetIndex(iIndex.kRef()),
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
                        })),
        };
}