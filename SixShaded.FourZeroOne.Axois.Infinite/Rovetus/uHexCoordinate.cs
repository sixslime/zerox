namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus;

public interface uHexCoordinate : IRovetu
{
    public static readonly Rovu<uHexCoordinate, Number> R = new(Axoi.Du, "r");
    public static readonly Rovu<uHexCoordinate, Number> U = new(Axoi.Du, "u");
    public static readonly Rovu<uHexCoordinate, Number> D = new(Axoi.Du, "d");
}