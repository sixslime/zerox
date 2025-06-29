namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

public interface uGameConfiguration : IRovetu
{
    public static readonly Rovu<uGameConfiguration, IMulti<IRoveggi<uPlayerDeclaration>>> PLAYERS = new(Axoi.Du, "players");
    public static readonly Rovu<Identifiers.uHexCoordinate, IMulti<IRoveggi<uHexDeclaration>>> MAP = new(Axoi.Du, "players");
}