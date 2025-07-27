namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Switch<RIn, ROut>
    where ROut : class, Rog
    where RIn : class, Rog
{
    public static Korvessa<RIn, IMulti<MetaFunction<RIn, Bool>>, IMulti<MetaFunction<ROut>>, ROut> Construct(IKorssa<RIn> input, IKorssa<IMulti<MetaFunction<RIn, Bool>>> matchers, IKorssa<IMulti<MetaFunction<ROut>>> returners) =>
        new(input, matchers, returners)
        {
            Du = Axoi.Korvedu("Switch"),
            Definition =
                (_, iInput, iMatchers, iReturners) =>
                    Core.kMetaFunctionRecursive<Number, ROut>(
                        [],
                        (iRecurse, iIndex) =>
                            iMatchers.kRef()
                                .kGetIndex(iIndex.kRef())
                                .kExecuteWith(
                                new()
                                {
                                    A = iInput.kRef()
                                })
                                .kIfTrue<ROut>(
                                new()
                                {
                                    Then = iReturners.kRef().kGetIndex(iIndex.kRef()).kExecute(),
                                    Else =
                                        iRecurse.kRef()
                                            .kExecuteWith(
                                            new()
                                            {
                                                A = iIndex.kRef().kAdd(1.kFixed())
                                            })
                                }))
                        .kExecuteWith(
                        new()
                        {
                            A = 1.kFixed()
                        })

        };
}