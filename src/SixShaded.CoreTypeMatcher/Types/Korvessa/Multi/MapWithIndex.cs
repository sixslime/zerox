namespace SixShaded.CoreTypeMatcher.Types.Korvessa.Multi;

public record MapWithIndex : IKorssaType
{
    public required RoggiTypeInfo SourceType { get; init; }
    public required RoggiTypeInfo ResultType { get; init; }
}