namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public record Switch<R>(IKorssa<IMulti<MetaFunction<R>>> statements) : Korvessa<IMulti<MetaFunction<R>>, R>(statements)
    where R : class, Rog
{
    protected override RecursiveMetaDefinition<IMulti<MetaFunction<R>>, R> InternalDefinition() =>
        (_, iStatements) =>
            Core.kMetaFunctionRecursive<Roggis.Number, R>(
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
                });
}
