namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

public interface uPlayerDeclaration : IRovetu
{
    public static readonly Rovu<uPlayerDeclaration, Number> HAND_SIZE = new(Axoi.Du, "hand_size");
    public static readonly Rovu<uPlayerDeclaration, IMulti<IRoveggi<Constructs.uAbility>>> DECK = new(Axoi.Du, "deck");
}