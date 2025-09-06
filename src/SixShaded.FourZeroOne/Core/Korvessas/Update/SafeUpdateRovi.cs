namespace SixShaded.FourZeroOne.Core.Korvessas.Update;

using Korvessa.Defined;
using Roggis;
using Syntax;

public record SafeUpdateRovi<C, R>(IKorssa<IRoveggi<C>> roveggi, IKorssa<MetaFunction<R, R>> updateFunction) : Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>>(roveggi, updateFunction)
    where C : IRovetu
    where R : class, Rog
{
    public required IRovu<C, R> Rovu { get; init; }
    protected override RecursiveMetaDefinition<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> InternalDefinition() =>
        (_, iHolder, iUpdateFunction) =>
            Core.kSubEnvironment<IRoveggi<C>>(
            new()
            {
                Environment =
                [
                    iUpdateFunction.kRef()
                        .kExecuteWith(
                        new()
                        {
                            A = iHolder.kRef().kGetRovi(Rovu),
                        })
                        .kAsVariable(out var iValue)
                ],
                Value =
                    iValue.kRef()
                        .kKeepNolla(
                        () => iHolder.kRef()
                            .kWithRovi(Rovu, iValue.kRef()))
            });
}