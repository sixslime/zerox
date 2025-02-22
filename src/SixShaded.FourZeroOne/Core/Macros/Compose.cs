namespace SixShaded.FourZeroOne.Core.Macros;

using Resolutions;
using Syntax;

public static class Compose<C>
    where C : ICompositionType, new()
{
    public static Macro<ICompositionOf<C>> Construct() => new()
    {
        Label = Package.Label("Compose"),
        Definition = new Tokens.Fixed<ICompositionOf<C>>(new CompositionOf<C>()).tMetaBoxed().Resolution,
    };
}