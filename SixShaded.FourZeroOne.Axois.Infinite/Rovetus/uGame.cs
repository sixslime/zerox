namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus;

using Identifier;

public interface uGame : IConcreteRovetu
{
    public static readonly Rovu<uGame, Number> TURNS_WITHOUT_PROGRESSION = new(Axoi.Du, "turns_without_progression");
    public static readonly Rovu<uGame, Number> TURN_INDEX = new(Axoi.Du, "turn_index");
    public static readonly Rovu<uGame, Number> ROTATION_COUNT = new(Axoi.Du, "rotation_count");
    public static readonly Rovu<uGame, IMulti<IRoveggi<uPlayerIdentifier>>> TURN_ORDER = new(Axoi.Du, "turn_order");
    public static readonly Rovu<uGame, IRoveggi<uPlayerIdentifier>> CURRENT_PLAYER = new(Axoi.Du, "current_player");
}