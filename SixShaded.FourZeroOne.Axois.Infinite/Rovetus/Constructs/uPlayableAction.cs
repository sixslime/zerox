
namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs;

using Identifier;

public interface uPlayableAction : IConcreteRovetu
{
    /// <summary>
    /// Returns the *function* that creates a resolved played action.<br></br>
    /// Returns nolla if cannot be played by the given player.
    /// </summary>
    public static readonly Rovu<uPlayableAction, MetaFunction<IRoveggi<uPlayerIdentifier>, MetaFunction<IRoveggi<Resolved.uResolvedAction>>>> FOR_PLAYER = new(Axoi.Du, "for_player");
}