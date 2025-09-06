namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record Distinct : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}