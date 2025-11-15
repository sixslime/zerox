namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;

public interface uResolvedPositionalMove : IConcreteRovetu, uResolvedMove
{
    public static new readonly Rovu<uResolvedPositionalMove, IRoveggi<Move.uPositionalMove>> MOVE = new("move");
    public static readonly ImplementationStatement<uResolvedPositionalMove> __IMPLEMENTS =
        c =>
            c.ImplementGet(
            uResolvedMove.MOVE,
            iSelf => iSelf.kRef().kGetRovi(MOVE));
}