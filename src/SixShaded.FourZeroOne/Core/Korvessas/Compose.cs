namespace SixShaded.FourZeroOne.Core.Korvessas;

using Korvessa.Defined;
using Syntax;

public static class Compose<C>
    where C : IConcreteRovetu
{
    public static Korvessa<IRoveggi<C>> Construct() =>
        new()
        {
            Du = Axoi.Korvedu("Compose"),
            Definition = _ => new Korssas.Fixed<IRoveggi<C>>(new Roveggi.Defined.Roveggi<C>()),
        };
}