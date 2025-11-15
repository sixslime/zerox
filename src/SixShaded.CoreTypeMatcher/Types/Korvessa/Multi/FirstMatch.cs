namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Multi;

public record FirstMatch : IKorssaType
{
    public required RoggiTypeInfo ElementType { get; init; }
}