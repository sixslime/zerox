namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public record Duplicate<R>(IKorssa<R> value, IKorssa<Number> count) : Korvessa<R, Number, Multi<R>>(value, count)
    where R : class, Rog
{
    protected override RecursiveMetaDefinition<R, Number, Multi<R>> InternalDefinition() =>
        (_, iValue, iCount) =>
            Core.kMetaFunctionRecursive<Number, Multi<R>>(
                [],
                (iRecurse, iIndex) =>
                    iIndex.kRef()
                        .kIsGreaterThan(iCount.kRef())
                        .kIfTrue<Multi<R>>(
                        new()
                        {
                            Then = Core.kNollaFor<Multi<R>>(),
                            Else =
                                Core.kMulti(
                                    [
                                        iValue.kRef().kYield(),
                                        iRecurse.kRef()
                                            .kExecuteWith(
                                            new()
                                            {
                                                A = iIndex.kRef().kAdd(1.kFixed()),
                                            })
                                    ])
                                    .kFlatten(),
                        }))
                .kExecuteWith(
                new()
                {
                    A = 1.kFixed(),
                });
}
