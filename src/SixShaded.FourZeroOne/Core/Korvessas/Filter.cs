namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Filter<R>
    where R : class, Rog
{
    public static Korvessa<IMulti<R>, MetaFunction<R, Bool>, Multi<R>> Construct(IKorssa<IMulti<R>> multi, IKorssa<MetaFunction<R, Bool>> predicate) =>
        new(multi, predicate)
        {
            Du = Axoi.Korvedu("Filter"),
            Definition =
                (_, iMulti, iPredicate) =>
                    Core.kMetaFunctionRecursive<Number, Multi<R>>(
                        [],
                        (iRecurse, iIndex) =>
                            iIndex.kRef()
                                .kIsGreaterThan(iMulti.kRef().kCount())
                                .kIfTrue<Multi<R>>(
                                new()
                                {
                                    Then = Core.kMulti<R>([]),
                                    Else =
                                        Core.kSubEnvironment<Multi<R>>(
                                            new()
                                            {
                                                Environment =
                                                [
                                                    iMulti.kRef()
                                                        .kGetIndex(iIndex.kRef())
                                                        .kAsVariable(out var iElement)
                                                ],
                                                Value =
                                                    iPredicate.kRef()
                                                        .kExecuteWith(
                                                        new()
                                                        {
                                                            A = iElement.kRef()
                                                        })
                                                        .kIfTrue<Multi<R>>(
                                                        new()
                                                        {
                                                            Then = iElement.kRef().kYield(),
                                                            Else = Core.kMulti<R>([])
                                                        })
                                            })
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