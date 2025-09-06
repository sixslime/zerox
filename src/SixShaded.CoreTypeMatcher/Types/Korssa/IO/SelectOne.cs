namespace SixShaded.CoreTypeMatcher.Types.Korssa.IO;

public record SelectOne : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}