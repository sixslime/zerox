namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Data;

using Identifier;

public interface uUnitData : IRovetu
{
    public static readonly Rovu<uUnitData, IRoveggi<uHexCoordinate>> POSITION = new(Axoi.Du, "position");
    public static readonly Rovu<uUnitData, IRoveggi<uUnitIdentifier>> ID = new(Axoi.Du, "id");
    public static readonly Rovu<uUnitData, IRoveggi<uPlayerIdentifier>> OWNER = new(Axoi.Du, "owner");
    public static readonly Rovu<uUnitData, IMulti<IRoveggi<Constructs.UnitEffects.uUnitEffect>>> EFFECTS = new(Axoi.Du, "effects");
}