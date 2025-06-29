namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Data;

public interface uUnitData : IRovetu, uPositioned
{
    public static readonly Rovu<uPlayerData, IRoveggi<Identifiers.uUnitIdentifier>> ID = new(Axoi.Du, "id");
    public static readonly Rovu<uPlayerData, IRoveggi<Identifiers.uPlayerIdentifier>> OWNER = new(Axoi.Du, "owner");
}