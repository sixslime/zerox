namespace SixShaded.CoreTypeMatcher.Types.Korssa.Memory;

public record Reference : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}