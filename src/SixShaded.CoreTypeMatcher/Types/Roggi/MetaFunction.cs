namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record MetaFunction : IRoggiType
{
    public required RoggiTypeInfo[] ArgTypes { get; init; }
    public required RoggiTypeInfo OutputType { get; init; }
}