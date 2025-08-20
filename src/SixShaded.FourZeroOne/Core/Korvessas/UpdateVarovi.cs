namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using Roveggi;

public static class UpdateVarovi<C, RKey, RVal>
    where C : IRovetu
    where RKey : class, Rog
    where RVal : class, Rog
{
    public static Korvessa<IRoveggi<C>, RKey, MetaFunction<RVal, RVal>, IRoveggi<C>> Construct(IKorssa<IRoveggi<C>> roveggi, IKorssa<RKey> key, IKorssa<MetaFunction<RVal, RVal>> updateFunction, IVarovu<C, RKey, RVal> varovu) =>
        new(roveggi, key, updateFunction)
        {
            Du = Axoi.Korvedu("UpdateVarovi"),
            CustomData = [varovu],
            Definition =
                (_, iHolder, iKey, iUpdateFunction) =>
                    iHolder.kRef()
                        .kWithVarovi(
                        varovu,
                        iKey.kRef(),
                        iUpdateFunction.kRef()
                            .kExecuteWith(
                            new()
                            {
                                A = iHolder.kRef().kGetVarovi(varovu, iKey.kRef()),
                            })),
        };
}