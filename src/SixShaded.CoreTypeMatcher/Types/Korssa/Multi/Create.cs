namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record Create : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}