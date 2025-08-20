namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

public interface uHexCoordinates : IRovetu
{
    public static readonly Rovu<uHexCoordinates, Number> R = new(Axoi.Du, "r");
    public static readonly Rovu<uHexCoordinates, Number> U = new(Axoi.Du, "u");
    public static readonly Rovu<uHexCoordinates, Number> D = new(Axoi.Du, "d");
}