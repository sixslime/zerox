namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class UpdateComponent<C, R>
    where C : IRovetu
    where R : class, Rog
{
    public static Korvessa<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>> Construct(IKorssa<IRoveggi<C>> roveggi, IKorssa<MetaFunction<R, R>> updateFunction, IRovu<C, R> component) => new(roveggi, updateFunction)
    {
        Du = Axoi.Korvedu("UpdateComponent"),
        CustomData = [component],
        Definition = Core.tMetaFunction<IRoveggi<C>, MetaFunction<R, R>, IRoveggi<C>>(
                (roveggiI, updateFunctionI) =>
                    roveggiI.tRef().kWithRovi(component,
                        updateFunctionI.tRef()
                            .tExecuteWith(new() { A = roveggiI.tRef().kGetRovi(component) })))
            .Roggi,
    };
}