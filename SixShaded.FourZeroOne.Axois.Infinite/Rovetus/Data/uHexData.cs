namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Data;

public interface uHexData : IRovetu, uPositioned
{
    public static readonly Rovu<uHexData, IRoveggi<Constructs.HexTypes.uHexType>> TYPE = new(Axoi.Du, "type");
}