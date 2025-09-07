namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Multi;

public record DistinctBy : IKorssaType
{
    public required RoggiTypeInfo ElementType { get; init; }
}