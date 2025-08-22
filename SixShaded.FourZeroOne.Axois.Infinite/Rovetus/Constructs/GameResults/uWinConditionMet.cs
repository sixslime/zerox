namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.GameResults;

using Identifier;
public interface uWinConditionMet : IConcreteRovetu, uGameResult
{
    public static readonly Varovu<uWinConditionMet, IRoveggi<uPlayerIdentifier>, IRoveggi<uWinChecks>> PLAYERS = new(Axoi.Du, "players");
}