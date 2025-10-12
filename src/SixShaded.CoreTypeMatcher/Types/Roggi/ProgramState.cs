namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record ProgramState : IRoggiType
{
    public required Func<Rog, FourZeroOne.FZOSpec.IMemoryFZO> MemoryGetter { get; init; }

}