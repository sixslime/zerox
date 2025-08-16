namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

// be careful, can generate infinite sequences, yada yada.
public static class Sequence<R>
    where R : class, Rog
{
    public static Korvessa<R, MetaFunction<R, Number, R>, Multi<R>> Construct(IKorssa<R> initialValue, IKorssa<MetaFunction<R, Number, R>> generator) =>
        new(initialValue, generator)
        {
            Du = Axoi.Korvedu("Sequence"),
            Definition =
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
                            B = 2.kFixed()
                        })
        };
}