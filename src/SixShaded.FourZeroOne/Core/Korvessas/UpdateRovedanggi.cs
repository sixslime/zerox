namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using Roveggi;

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
