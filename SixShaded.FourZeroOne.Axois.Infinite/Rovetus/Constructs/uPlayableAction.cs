namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

using Identifier;

public interface uPlayableAction : IConcreteRovetu
{
    /// <summary>
    /// Returns the energy cost of this action.<br></br>
    /// Return nolla if this action should not be playable (regardless of energy).
    /// </summary>
    public static readonly Rovu<uPlayableAction, MetaFunction<IRoveggi<uPlayerIdentifier>, Number>> COST_FUNCTION = new(Axoi.Du, "cost_function");
    public static readonly Rovu<uPlayableAction, MetaFunction<IRoveggi<uPlayerIdentifier>, Rog>> ACTION = new(Axoi.Du, "action");
}