namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Data;

using Identifier;

public interface uUnitData : IConcreteRovetu
{
    public static readonly Rovu<uUnitData, Number> HP = new("hp");
    public static readonly Rovu<uUnitData, IRoveggi<uHexIdentifier>> POSITION = new("position");
    public static readonly Rovu<uUnitData, IRoveggi<uPlayerIdentifier>> OWNER = new("owner");
    public static readonly Rovu<uUnitData, IMulti<IRoveggi<Constructs.uUnitEffect>>> EFFECTS = new("effects");
}