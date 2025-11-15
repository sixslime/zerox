namespace SixShaded.CoreTypeMatcher.Types.Korssa;

public record Nolla : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}