namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record Yield : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}