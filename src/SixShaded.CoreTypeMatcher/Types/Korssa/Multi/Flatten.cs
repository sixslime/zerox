namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record Flatten : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}