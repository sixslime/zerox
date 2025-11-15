namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;

public interface uResolvedAction : IConcreteRovetu, uResolved
{
    public static readonly Rovu<uResolvedAction, IRoveggi<Identifier.uPlayerIdentifier>> PLAYER = new("player");
    public static readonly Rovu<uResolvedAction, IRoveggi<uPlayableAction>> ACTION = new("action");
}