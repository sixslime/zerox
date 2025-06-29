namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

public interface uGameConfiguration : IRovetu
{
    public static readonly Rovu<uGameConfiguration, IMulti<IRoveggi<uPlayerDeclaration>>> PLAYERS = new(Axoi.Du, "players");
    public static readonly Varovu<uGameConfiguration, IRoveggi<Identifiers.uHexCoordinate>, IMulti<IRoveggi<Constructs.HexTypes.uHexType>>> MAP = new(Axoi.Du, "map");
    public static readonly Rovu<uGameConfiguration, IMulti<MetaFunction<Rog>>> MODIFIERS = new(Axoi.Du, "modifiers");
}