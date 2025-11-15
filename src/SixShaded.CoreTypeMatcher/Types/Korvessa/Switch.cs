namespace SixShaded.CoreTypeMatcher.Types.Korvessa;

public record Switch : IKorssaType
{
    public required RoggiTypeInfo OutputType { get; init; }
}