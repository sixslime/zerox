namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record GetSlice : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}