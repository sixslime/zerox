namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record GetIndex : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}