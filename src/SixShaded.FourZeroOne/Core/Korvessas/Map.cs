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
            Definition = Core.tMetaFunction<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>>(
                    (multiI, mapFunctionI) =>
                        Core.tMetaRecursiveFunction<Number, Multi<ROut>>(
                                (selfFunc, i) =>
                                    i.tRef().tIsGreaterThan(multiI.tRef().tCount())
                                        .tIfTrue<Multi<ROut>>(new()
                                        {
                                            Then = Core.tNollaFor<Multi<ROut>>(),
                                            Else = Core.tUnionOf(
                                            [
                                                mapFunctionI.tRef().tExecuteWith(
                                                    new() { A = multiI.tRef().tGetIndex(i.tRef()) }).tYield(),
                                                selfFunc.tRef().tExecuteWith(
                                                    new() { A = i.tRef().tAdd(1.tFixed()) }),
                                            ]),
                                        }))
                            .tExecuteWith(new() { A = 1.tFixed() }))
                .Roggi,
        };
}