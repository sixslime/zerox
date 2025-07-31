namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Data;

using Constructs.Ability;
using Identifier;

public interface uPlayerData : IConcreteRovetu
{
    public static readonly Rovu<uPlayerData, IMulti<IRoveggi<Constructs.uPlayableAction>>> AVAILABLE_ACTIONS = new(Axoi.Du, "available_actions");
    public static readonly Rovu<uPlayerData, IMulti<IRoveggi<uAbility>>> HAND = new(Axoi.Du, "hand");
    public static readonly Rovu<uPlayerData, IMulti<IRoveggi<uAbility>>> STACK = new(Axoi.Du, "stack");
    public static readonly Rovu<uPlayerData, Number> ENERGY = new(Axoi.Du, "energy");
    public static readonly Rovu<uPlayerData, Number> HAND_SIZE = new(Axoi.Du, "hand_size");
    public static readonly Rovu<uPlayerData, Number> CONTROL = new(Axoi.Du, "control");
    public static readonly Rovu<uPlayerData, Number> PERSPECTIVE_ROTATION = new(Axoi.Du, "perspective_rotation");
}