namespace SixShaded.CoreTypeMatcher.Types.Korvessa;

public record Compose : IKorssaType
{
    public required RovetuTypeInfo RovetuType { get; init; }
}