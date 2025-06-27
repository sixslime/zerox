namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus;

public interface uPlayerIdentifier : IRovetu
{
    public static readonly Rovu<uPlayerIdentifier, Number> NUMBER = new(Axoi.Du, "number");
}