namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record Intersect : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}