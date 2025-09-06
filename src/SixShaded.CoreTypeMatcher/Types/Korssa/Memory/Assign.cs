namespace SixShaded.CoreTypeMatcher.Types.Korssa.Memory;

public record Assign : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}