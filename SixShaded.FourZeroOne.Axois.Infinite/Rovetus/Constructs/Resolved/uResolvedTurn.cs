namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;

public interface uResolvedTurn : IConcreteRovetu, uResolved
{
    public static readonly Rovu<uResolvedTurn, IRoveggi<uTurn>> TURN = new(Axoi.Du, "turn");
    public static readonly Rovu<uResolvedTurn, IMulti<IRoveggi<uResolvedAction>>> ACTIONS = new(Axoi.Du, "actions");
}