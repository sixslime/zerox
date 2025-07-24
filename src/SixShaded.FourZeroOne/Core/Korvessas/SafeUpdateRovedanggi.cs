namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using Roveggi;

public static class SafeUpdateRovedanggi<R>
    where R : class, Rog
{
    public static Korvessa<IRoveggi<Rovedantu<R>>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> Construct(IKorssa<IRoveggi<Rovedantu<R>>> rovedanggi, IKorssa<MetaFunction<R, R>> updateFunction) =>
        new(rovedanggi, updateFunction)
        {
            Du = Axoi.Korvedu("SafeUpdateRovedanggi"),
            Definition =
                (_, iDan, iUpdateFunction) =>
                    Core.kSubEnvironment<Roggis.Instructions.Assign<R>>(
                    new()
                    {
                        Environment =
                        [
                            iUpdateFunction.kRef()
                                .kExecuteWith(
                                new()
                                {
                                    A = iDan.kRef().kGet(),
                                })
                                .kAsVariable(out var iValue)
                        ],
                        Value =
                            iValue.kRef()
                                .ksKeepNolla(
                                iDan.kRef()
                                    .kWrite(iValue.kRef()))
                    })
        };
}