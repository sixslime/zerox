namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Data;

public interface uHexData : IRovetu
{
    public static readonly Rovu<uHexData, IRoveggi<Identifiers.uHexCoordinate>> POSITION = new(Axoi.Du, "position");
    public static readonly Rovu<uHexData, IRoveggi<Constructs.HexTypes.uHexType>> TYPE = new(Axoi.Du, "type");
}