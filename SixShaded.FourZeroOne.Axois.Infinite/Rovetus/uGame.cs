namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus;

using Identifier;

public interface uGame : IConcreteRovetu
{
    public static readonly Rovu<uGame, ProgramState> LAST_ROTATION_STATE = new(Axoi.Du, "last_rotation_state");
    public static readonly Rovu<uGame, Number> DEAD_ROTATIONS = new(Axoi.Du, "dead_rotations");
    public static readonly Rovu<uGame, Number> TURN_INDEX = new(Axoi.Du, "turn_index");
    public static readonly Rovu<uGame, Number> ROTATION_COUNT = new(Axoi.Du, "rotation_count");
    public static readonly Rovu<uGame, IMulti<IRoveggi<uPlayerIdentifier>>> TURN_ORDER = new(Axoi.Du, "turn_order");
    public static readonly Rovu<uGame, IRoveggi<uPlayerIdentifier>> CURRENT_PLAYER = new(Axoi.Du, "current_player");
}