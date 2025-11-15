namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Data;

using Constructs.Ability;
using Identifier;

public interface uPlayerData : IConcreteRovetu
{
    public static readonly Rovu<uPlayerData, IMulti<IRoveggi<Constructs.uPlayableAction>>> PLAYABLE_ACTIONS = new("available_actions");
    public static readonly Rovu<uPlayerData, IMulti<IRoveggi<uAbility>>> HAND = new("hand");
    public static readonly Rovu<uPlayerData, IMulti<IRoveggi<uAbility>>> STACK = new("stack");
    public static readonly Rovu<uPlayerData, Number> ENERGY = new("energy");
    public static readonly Rovu<uPlayerData, Number> HAND_SIZE = new("hand_size");
    public static readonly Rovu<uPlayerData, Number> CONTROL = new("control");
    /// <summary>
    ///     clockwise rotations.
    /// </summary>
    public static readonly Rovu<uPlayerData, Number> PERSPECTIVE_ROTATION = new("perspective_rotation");
}