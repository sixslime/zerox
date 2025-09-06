namespace SixShaded.FourZeroOne.Core.Korvessas.Multi;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record DistinctBy<R>(IKorssa<IMulti<R>> source, IKorssa<MetaFunction<R, Rog>> keyFunction) : Korvessa<IMulti<R>, MetaFunction<R, Rog>, Multi<R>>(source, keyFunction)
    where R : class, Rog
{
    protected override RecursiveMetaDefinition<IMulti<R>, MetaFunction<R, Rog>, Multi<R>> InternalDefinition() =>
        (_, iSource, iKeyFunction) =>
            Core.kMetaFunctionRecursive<Multi<R>, IMulti<Rog>, Number, Multi<R>>(
                [],
                (iRecurse, iOut, iSeen, iIndex) =>
                    iIndex.kRef()
                        .kIsGreaterThan(iSource.kRef().kCount())
                        .kIfTrue<Multi<R>>(
                        new()
                        {
                            Then = iOut.kRef(),
                            Else =
                                Core.kSubEnvironment<Multi<R>>(
                                new()
                                {
                                    Environment =
                                    [
                                        iSource.kRef()
                                            .kGetIndex(iIndex.kRef())
                                            .kAsVariable(out var iElement),
                                        iKeyFunction.kRef()
                                            .kExecuteWith(
                                            new()
                                            {
                                                A = iElement.kRef(),
                                            })
                                            .kAsVariable(out var iKeyValue),
                                        iSeen.kRef()
                                            .kContains(iKeyValue.kRef())
                                            .kCatchNolla(() => true.kFixed())
                                            .kAsVariable(out var iIsDuplicate)
                                    ],
                                    Value =
                                        iRecurse.kRef()
                                            .kExecuteWith(
                                            new()
                                            {
                                                A =
                                                    iOut.kRef()
                                                        .kConcat(
                                                        iIsDuplicate.kRef()
                                                            .kIfTrue<Multi<R>>(
                                                            new()
                                                            {
                                                                Then = Core.kMulti<R>([]),
                                                                Else = iElement.kRef().kYield()
                                                            })),
                                                B =
                                                    iSeen.kRef()
                                                        .kConcat(
                                                        iIsDuplicate.kRef()
                                                            .kIfTrue<IMulti<Rog>>(
                                                            new()
                                                            {
                                                                Then = Core.kMulti<Rog>([]),
                                                                Else = iKeyValue.kRef().kYield()
                                                            })),
                                                C =
                                                    iIndex.kRef().kAdd(1.kFixed())
                                            })
                                })
                        }))
            .kExecuteWith(
            new()
            {
                A = Core.kMulti<R>([]),
                B = Core.kMulti<Rog>([]),
                C = 1.kFixed()
            });
}
