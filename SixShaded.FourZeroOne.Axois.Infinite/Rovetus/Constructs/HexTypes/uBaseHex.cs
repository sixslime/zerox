namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.HexTypes;

using Identifier;

public interface uBaseHex : IRovetu, uHexType
{
    public static readonly Rovu<uBaseHex, IRoveggi<uPlayerIdentifier>> OWNER = new(Axoi.Du, "owner");
}