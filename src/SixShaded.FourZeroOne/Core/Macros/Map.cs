namespace SixShaded.FourZeroOne.Core.Macros;

using Resolutions;
using Syntax;

public static class Map<RIn, ROut>
    where RIn : class, Res
    where ROut : class, Res
{
    public static Macro<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>> Construct(IToken<IMulti<RIn>> multi, IToken<MetaFunction<RIn, ROut>> mapFunction) =>
        new(multi, mapFunction)
        {
            Label = Package.Label("map"),
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
                .Resolution,
        };
}