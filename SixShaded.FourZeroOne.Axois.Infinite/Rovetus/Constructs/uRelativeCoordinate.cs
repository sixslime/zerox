namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

public interface uRelativeCoordinate : IRovetu
{
    public static readonly Rovu<uRelativeCoordinate, Number> R = new(Axoi.Du, "r");
    public static readonly Rovu<uRelativeCoordinate, Number> U = new(Axoi.Du, "u");
    public static readonly Rovu<uRelativeCoordinate, Number> D = new(Axoi.Du, "d");
}