namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record Contains : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}