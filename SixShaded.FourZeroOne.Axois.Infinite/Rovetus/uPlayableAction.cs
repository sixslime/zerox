namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus;

using Data;

public interface uPlayableAction : IRovetu
{
    public static readonly Rovu<uPlayableAction, Number> ENERGY_COST = new(Axoi.Du, "energy_cost");
    public static readonly Rovu<uPlayableAction, MetaFunction<IRoveggi<uPlayerData>, Rog>> ACTION = new(Axoi.Du, "action");
}