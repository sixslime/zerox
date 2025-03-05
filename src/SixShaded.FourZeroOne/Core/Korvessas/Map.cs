namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Map<RIn, ROut>
    where RIn : class, Rog
    where ROut : class, Rog
{
    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>> Construct(IKorssa<IMulti<RIn>> multi, IKorssa<MetaFunction<RIn, ROut>> mapFunction) =>
        new(multi, mapFunction)
        {
            Du = Axoi.Korvedu("map"),
            Definition = Core.kMetaFunction<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>>(
                    (multiI, mapFunctionI) =>
                        Core.kMetaFunctionRecursive<Number, Multi<ROut>>(
                                (selfFunc, i) =>
                                    i.kRef().kIsGreaterThan(multiI.kRef().kCount())
                                        .kIfTrue<Multi<ROut>>(new()
                                        {
                                            Then = Core.kNollaFor<Multi<ROut>>(),
                                            Else = Core.kUnionOf(
                                            [
                                                mapFunctionI.kRef().kExecuteWith(
                                                    new() { A = multiI.kRef().kGetIndex(i.kRef()) }).kYield(),
                                                selfFunc.kRef().kExecuteWith(
                                                    new() { A = i.kRef().kAdd(1.kFixed()) }),
                                            ]),
                                        }))
                            .kExecuteWith(new() { A = 1.kFixed() }))
                .Roggi,
        };
}