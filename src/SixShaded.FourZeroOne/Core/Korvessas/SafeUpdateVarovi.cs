namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using Roveggi;

public record SafeUpdateVarovi<C, RKey, RVal>(IKorssa<IRoveggi<C>> roveggi, IKorssa<RKey> key, IKorssa<MetaFunction<RVal, RVal>> updateFunction) : Korvessa<IRoveggi<C>, RKey, MetaFunction<RVal, RVal>, IRoveggi<C>>(roveggi, key, updateFunction)
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{
    public required IVarovu<C, RKey, RVal> Varovu { get; init; }

    protected override RecursiveMetaDefinition<IRoveggi<C>, RKey, MetaFunction<RVal, RVal>, IRoveggi<C>> InternalDefinition() =>
        (_, iHolder, iKey, iUpdateFunction) =>
            Core.kSubEnvironment<IRoveggi<C>>(
            new()
            {
                Environment =
                [
                    iUpdateFunction.kRef()
                        .kExecuteWith(
                        new()
                        {
                            A = iHolder.kRef().kGetVarovi(Varovu, iKey.kRef()),
                        })
                        .kAsVariable(out var iValue)
                ],
                Value =
                    iValue.kRef()
                        .kKeepNolla(
                        () => iHolder.kRef()
                            .kWithVarovi(Varovu, iKey.kRef(), iValue.kRef()))
            });
}