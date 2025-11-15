namespace SixShaded.CoreTypeMatcher.Types.Korvessa;

public record CatchNolla : IKorssaType
{
    public required RoggiTypeInfo RoggiType { get; init; }
}