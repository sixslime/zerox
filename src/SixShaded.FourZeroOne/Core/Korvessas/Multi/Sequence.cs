namespace SixShaded.FourZeroOne.Core.Korvessas.Multi;

using Korvessa.Defined;
using Roggis;
using Syntax;

// be careful, can generate infinite sequences, yada yada.
public record Sequence<R>(IKorssa<R> initialValue, IKorssa<MetaFunction<R, Number, R>> generator) : Korvessa<R, MetaFunction<R, Number, R>, Multi<R>>(initialValue, generator)
    where R : class, Rog
{
    protected override RecursiveMetaDefinition<R, MetaFunction<R, Number, R>, Multi<R>> InternalDefinition() =>
        (_, iInitialValue, iGenerator) =>
            Core.kMetaFunctionRecursive<R, Number, Multi<R>>(
                [],
                (iRecurse, iElement, iIndex) =>
                    iElement.kRef()
                        .kExists()
                        .kIfTrue<Multi<R>>(
                        new()
                        {
                            Then =
                                iElement.kRef()
                                    .kYield()
                                    .kConcat(
                                    iRecurse.kRef()
                                        .kExecuteWith(
                                        new()
                                        {
                                            A =
                                                iGenerator.kRef()
                                                    .kExecuteWith(
                                                    new()
                                                    {
                                                        A = iElement.kRef(),
                                                        B = iIndex.kRef()
                                                    }),
                                            B = iIndex.kRef().kAdd(1.kFixed())
                                        })),
                            Else = Core.kMulti<R>([])
                        }))
                .kExecuteWith(
                new()
                {
                    A = iInitialValue.kRef(),
                    B = 1.kFixed()
                });
}
