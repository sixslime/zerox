namespace SixShaded.CoreTypeMatcher.Types.Roggi.Instructions;

public record LoadProgramState : IRoggiType
{
    public required Func<Rog, FourZeroOne.Core.Roggis.ProgramState> ProgramStateGetter { get; init; }
}