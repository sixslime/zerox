namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

using Identifier;

public interface uGameConfiguration : IConcreteRovetu
{
    public static readonly Rovu<uGameConfiguration, IMulti<IRoveggi<uPlayerDeclaration>>> PLAYERS = new(Axoi.Du, "players");
    public static readonly Rovu<uGameConfiguration, IRoveggi<uMapDeclaration>> MAP = new(Axoi.Du, "map");
    public static readonly Rovu<uGameConfiguration, IMulti<MetaFunction<Rog>>> MODIFIERS = new(Axoi.Du, "modifiers");
}