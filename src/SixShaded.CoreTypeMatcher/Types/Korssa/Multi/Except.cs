namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record Except : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}