namespace SixShaded.CoreTypeMatcher.Types.Korssa.IO;

public record SelectMultiple : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}