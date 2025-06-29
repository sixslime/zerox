namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus;

public interface uPositioned : IRovetu
{
    public static readonly Rovu<uPositioned, IRoveggi<Identifiers.uHexCoordinate>> POSITION = new(Axoi.Du, "position");
}