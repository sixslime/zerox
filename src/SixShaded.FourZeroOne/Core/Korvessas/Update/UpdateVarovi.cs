namespace SixShaded.FourZeroOne.Core.Korvessas.Update;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record UpdateVarovi<C, RKey, RVal>(IKorssa<IRoveggi<C>> roveggi, IKorssa<RKey> key, IKorssa<MetaFunction<RVal, RVal>> updateFunction) : Korvessa<IRoveggi<C>, RKey, MetaFunction<RVal, RVal>, IRoveggi<C>>(roveggi, key, updateFunction)
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{
    public required IVarovu<C, RKey, RVal> Varovu { get; init; }

    protected override RecursiveMetaDefinition<IRoveggi<C>, RKey, MetaFunction<RVal, RVal>, IRoveggi<C>> InternalDefinition() =>
        (_, iHolder, iKey, iUpdateFunction) =>
            iHolder.kRef()
                .kWithVarovi(
                Varovu,
                iKey.kRef(),
                iUpdateFunction.kRef()
                    .kExecuteWith(
                    new()
                    {
                        A = iHolder.kRef().kGetVarovi(Varovu, iKey.kRef()),
                    }));
}