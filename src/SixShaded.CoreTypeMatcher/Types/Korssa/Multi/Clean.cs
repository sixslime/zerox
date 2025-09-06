namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record Clean : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}