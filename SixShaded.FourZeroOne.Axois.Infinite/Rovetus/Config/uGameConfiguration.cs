namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

public interface uGameConfiguration : IRovetu
{
    public static readonly Rovu<uGameConfiguration, IMulti<IRoveggi<uPlayerDeclaration>>> PLAYER_ROTATION = new(Axoi.Du, "player_rotation");
}