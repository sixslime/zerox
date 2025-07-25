namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class DistinctBy<R>
    where R : class, Rog
{
    public static Korvessa<IMulti<R>, MetaFunction<R, Rog>, Multi<R>> Construct(IKorssa<IMulti<R>> multi, IKorssa<MetaFunction<R, Rog>> keyFunction) =>
        new(multi, keyFunction)
        {
            Du = Axoi.Korvedu("DistinctBy"),
            Definition =
                (_, iMulti, iKeyFunction) =>
                    iMulti.kRef()
                        .kMapWithIndex(
                        [],
                        (iElement, iIndex) =>
                            Core.kMulti<Rog>(
                            iKeyFunction.kRef()
                                .kExecuteWith(
                                new()
                                {
                                    A = iElement.kRef()
                                }),
                            iIndex.kRef()
                            ))
                        
        };
}