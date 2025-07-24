namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;
using Roveggi;

public static class UpdateRovi<C, R>
    where C : IRovetu
    where R : class, Rog
{
    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> Construct(IKorssa<IRoveggi<C>> roveggi, IKorssa<MetaFunction<R, R>> updateFunction, IRovu<C, R> rovi) =>
        new(roveggi, updateFunction)
        {
            Du = Axoi.Korvedu("UpdateRovi"),
            CustomData = [rovi],
            Definition =
                (_, iHolder, iUpdateFunction) =>
                    iHolder.kRef()
                        .kWithRovi(
                        rovi,
                        iUpdateFunction.kRef()
                            .kExecuteWith(
                            new()
                            {
                                A = iHolder.kRef().kGetRovi(rovi),
                            }))),
        };
}