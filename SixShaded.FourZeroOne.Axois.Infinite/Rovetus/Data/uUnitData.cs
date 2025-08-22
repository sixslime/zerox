namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Data;

using Identifier;

public interface uUnitData : IConcreteRovetu
{
    public static readonly Rovu<uUnitData, Number> HP = new(Axoi.Du, "hp");
    public static readonly Rovu<uUnitData, IRoveggi<uHexIdentifier>> POSITION = new(Axoi.Du, "position");
    public static readonly Rovu<uUnitData, IRoveggi<uPlayerIdentifier>> OWNER = new(Axoi.Du, "owner");
    public static readonly Rovu<uUnitData, IMulti<IRoveggi<Constructs.uUnitEffect>>> EFFECTS = new(Axoi.Du, "effects");
}