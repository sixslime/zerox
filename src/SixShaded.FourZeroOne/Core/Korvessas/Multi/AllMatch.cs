namespace SixShaded.FourZeroOne.Core.Korvessas.Multi;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record AllMatch<R>(IKorssa<IMulti<R>> multi, IKorssa<MetaFunction<R, Bool>> predicate) : Korvessa<IMulti<R>, MetaFunction<R, Bool>, Bool>(multi, predicate)
    where R : class, Rog
{
    protected override RecursiveMetaDefinition<IMulti<R>, MetaFunction<R, Bool>, Bool> InternalDefinition() =>
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
                                    .kIfTrue<Bool>(
                                    new()
                                    {
                                        Then =
                                            iRecurse.kRef()
                                                .kExecuteWith(
                                                new()
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
                });
}