namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class AllMatch<R>
    where R : class, Rog
{
    public static Korvessa<IMulti<R>, MetaFunction<R, Bool>, Bool> Construct(IKorssa<IMulti<R>> multi, IKorssa<MetaFunction<R, Bool>> predicate) =>
        new(multi, predicate)
        {
            Du = Axoi.Korvedu("AllMatch"),
            Definition =
                (_, iMulti, iPredicate) =>
                    Core.kMetaFunctionRecursive<Number, Bool>(
                        [],
                        (iRecurse, iIndex) =>
                            iIndex.kRef()
                                .kIsGreaterThan(iMulti.kRef().kCount())
                                .kIfTrue<Bool>(
                                new()
                                {
                                    Then = true.kFixed(),
                                    Else =
                                        iPredicate.kRef()
                                            .kExecuteWith(
                                            new()
                                            {
                                                A = iMulti.kRef().kGetIndex(iIndex.kRef()),
                                            })
                                            .kIfTrue<Bool>(new()
                                            {
                                                Then = iRecurse.kRef()
                                                    .kExecuteWith(new()
                                                    {
                                                        A = iIndex.kRef().kAdd(1.kFixed())
                                                    }),
                                                Else = false.kFixed()
                                            })
                                }))
                        .kExecuteWith(
                        new()
                        {
                            A = 1.kFixed(),
                        }),
        };
}