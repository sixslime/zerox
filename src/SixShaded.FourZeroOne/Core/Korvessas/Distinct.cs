namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Distinct<R>
    where R : class, Rog
{
    public static Korvessa<IMulti<R>, Multi<R>> Construct(IKorssa<IMulti<R>> source) =>
        new(source)
        {
            Du = Axoi.Korvedu("Distinct"),
            Definition =
                (_, iSource) =>
                    Core.kMetaFunctionRecursive<Multi<R>, IMulti<R>, Number, Multi<R>>(
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
                                                iSeen.kRef()
                                                    .kContains(iElement.kRef())
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
                                                                    .kIfTrue<IMulti<R>>(
                                                                    new()
                                                                    {
                                                                        Then = Core.kMulti<R>([]),
                                                                        Else = iElement.kRef().kYield()
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
                            B = Core.kMulti<R>([]),
                            C = 1.kFixed()
                        })
        };
}