namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using Roveggi;

public static class SafeUpdateRovi<C, R>
    where C : IRovetu
    where R : class, Rog
{
    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> Construct(IKorssa<IRoveggi<C>> roveggi, IKorssa<MetaFunction<R, R>> updateFunction, IRovu<C, R> rovu) =>
        new(roveggi, updateFunction)
        {
            Du = Axoi.Korvedu("SafeUpdateRovi"),
            CustomData = [rovu],
            Definition =
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
                                    A = iHolder.kRef().kGetRovi(rovu),
                                })
                                .kAsVariable(out var iValue)
                        ],
                        Value =
                            iValue.kRef()
                                .ksKeepNolla(
                                () => iHolder.kRef()
                                    .kWithRovi(rovu, iValue.kRef()))
                    })
        };
}