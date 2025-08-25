namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public record Map<RIn, ROut>(IKorssa<IMulti<RIn>> multi, IKorssa<MetaFunction<RIn, ROut>> mapFunction) : Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>>(multi, mapFunction)
    where RIn : class, Rog
    where ROut : class, Rog
{
    protected override RecursiveMetaDefinition<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>> InternalDefinition() =>
        (_, iMulti, iMapFunction) =>
            Core.kMetaFunctionRecursive<Number, Multi<ROut>>(
                [],
                (iRecurse, iIndex) =>
                    iIndex.kRef()
                        .kIsGreaterThan(iMulti.kRef().kCount())
                        .kIfTrue<Multi<ROut>>(
                        new()
                        {
                            Then = Core.kMulti<ROut>([]),
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
                });
}
