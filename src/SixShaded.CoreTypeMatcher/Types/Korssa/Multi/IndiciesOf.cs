namespace SixShaded.CoreTypeMatcher.Types.Korssa.Multi;

public record IndiciesOf : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}