namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Switch<R>
where R : class, Rog
{
    public static Korvessa<IMulti<MetaFunction<R>>, R> Construct(IKorssa<IMulti<MetaFunction<R>>> statements) =>
        new(statements)
        {
            Du = Axoi.Korvedu("Switch"),
            Definition =
                (_, iStatements) =>
                    Core.kMetaFunctionRecursive<Number, R>(
                        [],
                        (iRecurse, iIndex) =>
                            iIndex.kRef()
                                .kIsGreaterThan(iStatements.kRef().kCount())
                                .kIfTrue<R>(
                                new()
                                {
                                    Then = Core.kNollaFor<R>(),
                                    Else =
                                        iStatements.kRef()
                                            .kGetIndex(iIndex.kRef())
                                            .kExecute()
                                            .kCatchNolla(
                                            () =>
                                                iRecurse.kRef()
                                                    .kExecuteWith(
                                                    new()
                                                    {
                                                        A = iIndex.kRef().kAdd(1.kFixed())
                                                    }))
                                }))
                        .kExecuteWith(
                        new()
                        {
                            A = 1.kFixed()
                        })
        };
}