namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.HexTypes;

using Identifier;

public interface uBaseHex : IConcreteRovetu, uHexType
{
    public static readonly Rovu<uBaseHex, IRoveggi<uPlayerIdentifier>> OWNER = new("owner");
}