namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Duplicate<R>
    where R : class, Rog
{
    public static Korvessa<R, Number, Multi<R>> Construct(IKorssa<R> value, IKorssa<Number> count) =>
        new(value, count)
        {
            Du = Axoi.Korvedu("Duplicate"),
            Definition =
                Core.kMetaFunction<R, Number, Multi<R>>(
                [],
                (valueI, countI) =>
                    Core.kMetaFunctionRecursive<Number, Multi<R>>(
                        [],
                        (selfFunc, i) =>
                            i.kRef()
                                .kIsGreaterThan(countI.kRef())
                                .kIfTrue<Multi<R>>(
                                new()
                                {
                                    Then = Core.kNollaFor<Multi<R>>(),
                                    Else =
                                        Core.kMulti(
                                        [
                                            valueI.kRef().kYield(),
                                            selfFunc.kRef()
                                                .kExecuteWith(
                                                new()
                                                {
                                                    A = i.kRef().kAdd(1.kFixed()),
                                                }),
                                        ]).kFlatten(),
                                }))
                        .kExecuteWith(
                        new()
                        {
                            A = 1.kFixed(),
                        })),
        };
}