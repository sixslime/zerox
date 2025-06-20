namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus;

using Core.Roggis;
using Roveggi.Defined;
public class HexCoordinate : Rovetu
{
    protected HexCoordinate() { }
    public static readonly Rovu<HexCoordinate, Number> R = new(Axoi.Du, "r");
    public static readonly Rovu<HexCoordinate, Number> U = new(Axoi.Du, "u");
    public static readonly Rovu<HexCoordinate, Number> D = new(Axoi.Du, "d");
}