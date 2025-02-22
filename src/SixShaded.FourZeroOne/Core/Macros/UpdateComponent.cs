
namespace SixShaded.FourZeroOne.Core.Macros
{
    public static class UpdateComponent<C, R>
        where C : ICompositionType
        where R : class, Res
    {
        public static Macro<ICompositionOf<C>, MetaFunction<R, R>, ICompositionOf<C>> Construct(IToken<ICompositionOf<C>> composition, IToken<MetaFunction<R, R>> updateFunction, IComponentIdentifier<C, R> component) => new(composition, updateFunction)
        {
            Label = Package.Label("UpdateComponent"),
            CustomData = [component],
            Definition = Core.tMetaFunction<ICompositionOf<C>, MetaFunction<R, R>, ICompositionOf<C>>(
                (compositionI, updateFunctionI) =>
                    compositionI.tRef().tWithComponent(component,
                        updateFunctionI.tRef()
                        .tExecuteWith(new() { A = compositionI.tRef().tGetComponent(component) })))
                .Resolution
        };
    }
