namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using Roveggi;

public static class UpdateRovedanggi<R>
    where R : class, Rog
{
    public static Korvessa<IRoveggi<Rovedantu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> Construct(IKorssa<IRoveggi<Rovedantu<R>>> rovedanggi, IKorssa<MetaFunction<R, R>> updateFunction) =>
        new(rovedanggi, updateFunction)
        {
            Du = Axoi.Korvedu("UpdateRovedanggi"),
            Definition =
                (_, iDan, iUpdateFunction) =>
                    iDan.kRef()
                        .kWrite(
                        iUpdateFunction.kRef()
                            .kExecuteWith(
                            new()
                            {
                                A = iDan.kRef().kRead(),
                            })),
        };
}