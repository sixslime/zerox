namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using Roveggi;

public static class UpdateRovedanggi<R>
    where R : class, Rog
{
    public static Korvessa<IRoveggi<Rovedantu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> Construct(IKorssa<IRoveggi<Rovedantu<R>>> memObj, IKorssa<MetaFunction<R, R>> updateFunction) =>
        new(memObj, updateFunction)
        {
            Du = Axoi.Korvedu("UpdateRovedanggi"),
            Definition =
                Core.kMetaFunction<IRoveggi<Rovedantu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>>(
                [],
                (iMemObj, iUpdateFunction) =>
                    iMemObj.kRef()
                        .kWrite(
                        iUpdateFunction.kRef()
                            .kExecuteWith(
                            new()
                            {
                                A = iMemObj.kRef().kGet(),
                            }))),
        };
}