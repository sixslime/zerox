namespace SixShaded.CoreTypeMatcher.Types.Korssa;

public record TryCast : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}