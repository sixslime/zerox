namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;

public interface uResolvedNumericalMove : IConcreteRovetu, uResolvedMove
{
    public static new readonly Rovu<uResolvedNumericalMove, IRoveggi<Move.uNumericalMove>> MOVE = new("move");
    public static readonly Rovu<uResolvedNumericalMove, Number> PATH_DISTANCE = new("path_distance");
    public static readonly ImplementationStatement<uResolvedNumericalMove> __IMPLEMENTS =
        c =>
            c.ImplementGet(
            uResolvedMove.MOVE,
            iSelf => iSelf.kRef().kGetRovi(MOVE));
}