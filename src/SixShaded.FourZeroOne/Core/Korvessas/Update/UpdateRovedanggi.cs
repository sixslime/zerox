namespace SixShaded.FourZeroOne.Core.Korvessas.Update;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record UpdateRovedanggi<R>(IKorssa<IRoveggi<Rovedantu<R>>> rovedanggi, IKorssa<MetaFunction<R, R>> updateFunction) : Korvessa<IRoveggi<Rovedantu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>>(rovedanggi, updateFunction)
    where R : class, Rog
{
    protected override RecursiveMetaDefinition<IRoveggi<Rovedantu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> InternalDefinition() =>
        (_, iDan, iUpdateFunction) =>
            iDan.kRef()
                .kWrite(
                iUpdateFunction.kRef()
                    .kExecuteWith(
                    new()
                    {
                        A = iDan.kRef().kRead(),
                    }));
}
