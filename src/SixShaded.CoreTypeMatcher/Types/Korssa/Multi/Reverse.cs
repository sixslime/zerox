namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record Reverse : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}