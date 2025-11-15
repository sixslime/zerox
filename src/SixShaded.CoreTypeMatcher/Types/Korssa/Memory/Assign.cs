namespace SixShaded.CoreTypeMatcher.Types.Korssa.Memory;

public record Assign : IKorssaType
{
    public required Func<Kor, RodaInfo> RodaInfoGetter { get; init; }
    public required RoggiTypeInfo RoggiType { get; init; }
}