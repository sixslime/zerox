namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Multi;

public record Sequence : IKorssaType
{
    public required RoggiTypeInfo ElementType { get; init; }
}