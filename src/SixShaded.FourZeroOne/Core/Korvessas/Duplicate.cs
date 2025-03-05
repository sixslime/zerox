namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Duplicate<R>
    where R : class, Rog
{
    public static Korvessa<R, Number, Multi<R>> Construct(IKorssa<R> value, IKorssa<Number> count) => new(value, count)
    {
        Du = Axoi.Korvedu("Duplicate"),
        Definition = Core.tMetaFunction<R, Number, Multi<R>>(
                (valueI, countI) =>
                    Core.tMetaRecursiveFunction<Number, Multi<R>>(
                            (selfFunc, i) =>
                                i.tRef().tIsGreaterThan(countI.tRef())
                                    .kIfTrue<Multi<R>>(new()
                                    {
                                        Then = Core.tNollaFor<Multi<R>>(),
                                        Else = Core.tUnionOf(
                                        [
                                            valueI.tRef().tYield(),
                                            selfFunc.tRef().tExecuteWith(new() { A = i.tRef().tAdd(1.kFixed()) }),
                                        ]),
                                    }))
                        .tExecuteWith(new() { A = 1.kFixed() }))
            .Roggi,
    };
}