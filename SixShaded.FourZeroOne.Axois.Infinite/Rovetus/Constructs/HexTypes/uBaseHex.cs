namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.HexTypes;

public interface uBaseHex : IRovetu, uHexType
{
    public static readonly Rovu<uBaseHex, IRoveggi<Identifiers.uPlayerIdentifier>> OWNER = new(Axoi.Du, "owner");
}