namespace SixShaded.FourZeroOne.Core.Korvessas.Update;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record UpdateRovi<C, R>(IKorssa<IRoveggi<C>> roveggi, IKorssa<MetaFunction<R, R>> updateFunction) : Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>>(roveggi, updateFunction)
    where C : IRovetu
    where R : class, Rog
{
    public required IRovu<C, R> Rovu { get; init; }
    protected override RecursiveMetaDefinition<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> InternalDefinition() =>
        (_, iHolder, iUpdateFunction) =>
            iHolder.kRef()
                .kWithRovi(
                Rovu,
                iUpdateFunction.kRef()
                    .kExecuteWith(
                    new()
                    {
                        A = iHolder.kRef().kGetRovi(Rovu),
                    }));
}