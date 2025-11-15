namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

using Identifier;

public interface uGameConfiguration : IConcreteRovetu
{
    public static readonly Rovu<uGameConfiguration, IMulti<IRoveggi<uPlayerDeclaration>>> PLAYERS = new("players");
    public static readonly Rovu<uGameConfiguration, IRoveggi<uMapDeclaration>> MAP = new("map");
    public static readonly Rovu<uGameConfiguration, IMulti<MetaFunction<Rog>>> MODIFIERS = new("modifiers");
}