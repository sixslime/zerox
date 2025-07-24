namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

using Constructs.Ability;

public interface uPlayerDeclaration : IConcreteRovetu
{
    public static readonly Rovu<uPlayerDeclaration, IMulti<IRoveggi<uAbility>>> DECK = new(Axoi.Du, "deck");
    public static readonly Rovu<uPlayerDeclaration, IMulti<MetaFunction<IRoveggi<Data.uPlayerData>, Rog>>> MODIFIERS = new(Axoi.Du, "modifiers");
}