namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Multi;

public record Concat : IKorssaType
{
    public required RoggiTypeInfo ElementType { get; init; }
}