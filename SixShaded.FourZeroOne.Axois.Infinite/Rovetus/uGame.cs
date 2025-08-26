namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus;

using Identifier;

public interface uGame : IConcreteRovetu
{
    public static readonly Rovu<uGame, ProgramState> LAST_ROTATION_STATE = new("last_rotation_state");
    public static readonly Rovu<uGame, Number> DEAD_ROTATIONS = new("dead_rotations");
    public static readonly Rovu<uGame, Number> TURN_INDEX = new("turn_index");
    public static readonly Rovu<uGame, Number> ROTATION_COUNT = new("rotation_count");
    public static readonly Rovu<uGame, IMulti<IRoveggi<uPlayerIdentifier>>> TURN_ORDER = new("turn_order");
    /// <summary>
    ///     Only set by AllowPlay, is not explicitly part of game information but is an indicator of which player is
    ///     acting/playing.
    /// </summary>
    public static readonly Rovu<uGame, IRoveggi<uPlayerIdentifier>> CURRENT_PLAYER = new("current_player");
}