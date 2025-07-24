namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using Roveggi;

public static class SafeUpdateVarovi<C, RKey, RVal>
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{
    public static Korvessa<IRoveggi<C>, RKey, MetaFunction<RVal, RVal>, IRoveggi<C>> Construct(IKorssa<IRoveggi<C>> roveggi, IKorssa<RKey> key, IKorssa<MetaFunction<RVal, RVal>> updateFunction, IVarovu<C, RKey, RVal> varovu) =>
        new(roveggi, key, updateFunction)
        {
            Du = Axoi.Korvedu("SafeUpdateVarovi"),
            CustomData = [varovu],
            Definition =
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
                                    A = iHolder.kRef().kGetVarovi(varovu, iKey.kRef()),
                                })
                                .kAsVariable(out var iValue)
                        ],
                        Value =
                            iValue.kRef()
                                .ksRemapNonNolla(
                                iHolder.kRef()
                                    .kWithVarovi(varovu, iKey.kRef(), iValue.kRef()))
                    })
        };
}