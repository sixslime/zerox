namespace SixShaded.FourZeroOne.Core.Korvessas;

using Roggis;
using Korvessa.Defined;
using Syntax;

public static class Compose<C>
    where C : IRoveggitu
{
    public static Korvessa<IRoveggi<C>> Construct() => new() { Du = Axoi.Korvedu("compose"), Definition = new Korssas.Fixed<IRoveggi<C>>(new Roveggi<C>()).tMetaBoxed().Roggi };
}