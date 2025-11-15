namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record Union : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}