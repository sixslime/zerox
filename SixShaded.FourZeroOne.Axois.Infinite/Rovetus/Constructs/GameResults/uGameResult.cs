namespace SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.GameResults;

public interface uGameResult : IRovetu
{
    public static readonly Rovu<uGameResult, ProgramState> FINAL_STATE = new(Axoi.Du, "final_state");
}