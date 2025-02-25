namespace SixShaded.FourZeroOne.Core.Korvessas;

// now that korvessas are data driven, 'decompose' can be functionally replaced with handler korvessas
using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Decompose<D, R>
    where D : IDecomposableRoveggitu<D, R>, new()
    where R : class, Rog
{
    public static Korvessa<IRoveggi<D>, R> Construct(IKorssa<IRoveggi<D>> roveggi) => new(roveggi) { Du = Axoi.Korvedu("Decompose"), Definition = new D().DecomposeFunction };
}

public static class UpdateMemoryObject<R>
    where R : class, Rog
{
    public static Korvessa<IMemoryObject<R>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>> Construct(IKorssa<IMemoryObject<R>> address, IKorssa<MetaFunction<R, R>> updateFunction) => new(address, updateFunction)
    {
        Du = Axoi.Korvedu("UpdateMemoryObject"),
        Definition = Core.tMetaFunction<IMemoryObject<R>, MetaFunction<R, R>, Roggis.Instructions.Assign<R>>(
                (addressI, updateFunctionI) =>
                    addressI.tRef().tMemoryWrite(
                        updateFunctionI.tRef()
                            .tExecuteWith(new() { A = addressI.tRef().tMemoryGet() })))
            .Roggi,
    };
}