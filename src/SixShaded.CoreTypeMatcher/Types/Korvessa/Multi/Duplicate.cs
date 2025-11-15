namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Multi;

public record Duplicate : IKorssaType
{
    public required RoggiTypeInfo ElementType { get; init; }
}