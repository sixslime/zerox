namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Data;

using Identifier;

public interface uHexData : IRovetu
{
    public static readonly Rovu<uHexData, IRoveggi<uHexCoordinate>> POSITION = new(Axoi.Du, "position");
    public static readonly Rovu<uHexData, IRoveggi<Constructs.HexTypes.uHexType>> TYPE = new(Axoi.Du, "type");
}