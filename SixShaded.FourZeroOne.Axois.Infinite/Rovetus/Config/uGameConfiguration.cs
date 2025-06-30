namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

using Identifier;

public interface uGameConfiguration : IRovetu
{
    public static readonly Varovu<uGameConfiguration, IRoveggi<uPlayerIdentifier>, IRoveggi<uPlayerDeclaration>> PLAYERS = new(Axoi.Du, "players");
    public static readonly Varovu<uGameConfiguration, IRoveggi<uHexCoordinate>, IMulti<IRoveggi<Constructs.HexTypes.uHexType>>> MAP = new(Axoi.Du, "map");
    public static readonly Rovu<uGameConfiguration, IMulti<IRoveggi<uPlayerIdentifier>>> TURN_ORDER = new(Axoi.Du, "turn_order");
    public static readonly Rovu<uGameConfiguration, IMulti<MetaFunction<Rog>>> MODIFIERS = new(Axoi.Du, "modifiers");
}