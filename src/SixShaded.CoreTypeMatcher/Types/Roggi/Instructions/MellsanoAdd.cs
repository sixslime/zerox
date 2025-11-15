namespace SixShaded.CoreTypeMatcher.Types.Roggi.Instructions;

public record MellsanoAdd : IRoggiType
{
    public required Func<Rog, Mel> MellsanoGetter { get; init; }
}