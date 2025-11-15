namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Multi;

public record Filter : IKorssaType
{
    public required RoggiTypeInfo ElementType { get; init; }
}