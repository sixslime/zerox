namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

public interface uPlayerDeclaration : IRovetu
{
    public static readonly Rovu<uPlayerDeclaration, IMulti<IRoveggi<Constructs.uAbility>>> DECK = new(Axoi.Du, "deck");
    public static readonly Rovu<uPlayerDeclaration, IMulti<MetaFunction<IRoveggi<Data.uPlayerData>, Rog>>> MODIFIERS = new(Axoi.Du, "modifiers");
}