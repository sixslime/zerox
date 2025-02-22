namespace SixShaded.FourZeroOne.Core.Macros;

// now that macros are data driven, 'decompose' can be functionally replaced with handler macros
using Resolutions;
using Syntax;

public static class Decompose<D, R>
    where D : IDecomposableType<D, R>, new()
    where R : class, Res
{
    public static Macro<ICompositionOf<D>, R> Construct(IToken<ICompositionOf<D>> composition) => new(composition)
    {
        Label = Package.Label("Decompose"),
        Definition = new D().DecompositionFunction,
    };
}
public static class UpdateMemoryObject<R>
    where R : class, Res
{
    public static Macro<IMemoryObject<R>, MetaFunction<R, R>, Resolutions.Instructions.Assign<R>> Construct(IToken<IMemoryObject<R>> address, IToken<MetaFunction<R, R>> updateFunction) => new(address, updateFunction)
    {
        Label = Package.Label("UpdateMemoryObject"),
        Definition = Core.tMetaFunction<IMemoryObject<R>, MetaFunction<R, R>, Resolutions.Instructions.Assign<R>>(
                (addressI, updateFunctionI) =>
                    addressI.tRef().tDataWrite(
                        updateFunctionI.tRef()
                            .tExecuteWith(new() { A = addressI.tRef().tDataGet() })))
            .Resolution,
    };
}