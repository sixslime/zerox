namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Multi;

public record AnyMatch : IKorssaType
{
    public required RoggiTypeInfo ElementType { get; init; }
}