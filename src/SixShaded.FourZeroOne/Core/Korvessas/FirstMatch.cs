namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class FirstMatch<R>
    where R : class, Rog
{
    public static Korvessa<IMulti<R>, MetaFunction<R, Bool>, R> Construct(IKorssa<IMulti<R>> multi, IKorssa<MetaFunction<R, Bool>> predicate) =>
        new(multi, predicate)
        {
            Du = Axoi.Korvedu("FirstMatch"),
            Definition =
                (_, iMulti, iPredicate) =>
                    Core.kMetaFunctionRecursive<Number, R>(
                        [],
                        (iRecurse, iIndex) =>
                            iIndex.kRef()
                                .kIsGreaterThan(iMulti.kRef().kCount())
                                .kIfTrue<R>(
                                new()
                                {
                                    Then = Core.kNollaFor<R>(),
                                    Else =
                                        Core.kSubEnvironment<R>(
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
                                                    .kIfTrue<R>(
                                                    new()
                                                    {
                                                        Then = iElement.kRef(),
                                                        Else =
                                                            iRecurse.kRef()
                                                                .kExecuteWith(
                                                                new()
                                                                {
                                                                    A = iIndex.kRef().kAdd(1.kFixed()),
                                                                })
                                                    })
                                        })

                                }))
                        .kExecuteWith(
                        new()
                        {
                            A = 1.kFixed(),
                        }),
        };
}