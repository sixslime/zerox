namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using Roveggi;

public static class SafeUpdateRovi<C, R>
    where C : IRovetu
    where R : class, Rog
{
    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> Construct(IKorssa<IRoveggi<C>> roveggi, IKorssa<MetaFunction<R, R>> updateFunction, IRovu<C, R> rovi) =>
        new(roveggi, updateFunction)
        {
            Du = Axoi.Korvedu("SafeUpdateRovi"),
            CustomData = [rovi],
            Definition =
                Core.kMetaFunction<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>>(
                [],
                (iDan, iUpdateFunction) =>
                    Core.kSubEnvironment<IRoveggi<C>>(
                    new()
                    {
                        Environment =
                        [
                            iUpdateFunction.kRef()
                                .kExecuteWith(
                                new()
                                {
                                    A = iDan.kRef().kGetRovi(rovi),
                                })
                                .kAsVariable(out var iValue)
                        ],
                        Value =
                            iValue.kRef()
                                .ksKeepNolla(
                                iDan.kRef()
                                    .kWithRovi(rovi, iValue.kRef()))
                    }))
        };
}