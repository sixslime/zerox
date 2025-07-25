namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;

public interface uResolvedMove : IRovetu, uResolved
{
    public static readonly AbstractGetRovu<uResolvedMove, IRoveggi<Move.uMove>> MOVE = new(Axoi.Du, "move");
    public static readonly Rovu<uResolvedMove, IRoveggi<Identifier.uHexCoordinate>> FROM = new(Axoi.Du, "from");
    public static readonly Rovu<uResolvedMove, IRoveggi<Identifier.uHexCoordinate>> DESTINATION = new(Axoi.Du, "destination");
}