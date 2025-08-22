namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Config;

using Identifier;

public interface uMapDeclaration : IConcreteRovetu
{
    public static readonly Varovu<uMapDeclaration, IRoveggi<uHexIdentifier>, IRoveggi<Constructs.HexTypes.uHexType>> HEXES = new(Axoi.Du, "hexes");
    public static readonly Varovu<uMapDeclaration, IRoveggi<uHexIdentifier>, Number> UNIT_SPAWNS = new(Axoi.Du, "unit_spawns");
}