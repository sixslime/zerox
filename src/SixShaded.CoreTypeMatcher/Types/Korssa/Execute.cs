namespace SixShaded.CoreTypeMatcher.Types.Korssa;

public record Execute : IKorssaType
{
    public required RoggiTypeInfo MetaOutputType { get; init; }
    public required RoggiTypeInfo[] MetaArgTypes { get; init; }
}