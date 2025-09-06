namespace SixShaded.FourZeroOne.Core.Korvessas.Multi;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record FirstMatch<R>(IKorssa<IMulti<R>> multi, IKorssa<MetaFunction<R, Bool>> predicate) : Korvessa<IMulti<R>, MetaFunction<R, Bool>, R>(multi, predicate)
    where R : class, Rog
{
    protected override RecursiveMetaDefinition<IMulti<R>, MetaFunction<R, Bool>, R> InternalDefinition() =>
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
                });
}
