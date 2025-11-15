namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

public interface uHexCoordinates : IRovetu
{
    public static readonly Rovu<uHexCoordinates, Number> R = new("r");
    public static readonly Rovu<uHexCoordinates, Number> U = new("u");
    public static readonly Rovu<uHexCoordinates, Number> D = new("d");
}