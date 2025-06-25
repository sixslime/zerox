namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus;

public interface uPlayableAction : IRovetu
{
    public static readonly Rovu<uPlayableAction, Number> ENERGY_COST = new(Axoi.Du, "energy_cost");
    public static readonly Rovu<uPlayableAction, MetaFunction<Rog>> ACTION = new(Axoi.Du, "action");
}