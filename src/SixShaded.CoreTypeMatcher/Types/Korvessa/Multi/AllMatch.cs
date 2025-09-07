namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Multi;

public record AllMatch : IKorssaType
{
    public required RoggiTypeInfo ElementType { get; init; }
}