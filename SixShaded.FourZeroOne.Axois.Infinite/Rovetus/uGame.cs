namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus;

public interface uGame : IRovetu
{
    public static readonly Rovu<uGame, Number> TURN_INDEX = new(Axoi.Du, "turn_index");
    public static readonly Rovu<uGame, Number> ROTATION_COUNT = new(Axoi.Du, "rotation_count");
}