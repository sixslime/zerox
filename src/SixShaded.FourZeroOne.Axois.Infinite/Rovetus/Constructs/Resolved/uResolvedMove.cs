namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;

public interface uResolvedMove : IRovetu, uResolved
{
    public static readonly AbstractGetRovu<uResolvedMove, IRoveggi<Move.uMove>> MOVE = new("move");
    public static readonly Rovu<uResolvedMove, IRoveggi<Identifier.uUnitIdentifier>> SUBJECT = new("subject");
    public static readonly Rovu<uResolvedMove, IRoveggi<Identifier.uHexIdentifier>> FROM = new("from");
    public static readonly Rovu<uResolvedMove, IRoveggi<Identifier.uHexIdentifier>> DESTINATION = new("destination");
}