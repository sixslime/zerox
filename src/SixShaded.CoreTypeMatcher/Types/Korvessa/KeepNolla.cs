namespace SixShaded.CoreTypeMatcher.Types.Korvessa;

public record KeepNolla : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}