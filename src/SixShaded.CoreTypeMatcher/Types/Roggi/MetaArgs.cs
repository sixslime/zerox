namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record MetaArgs : IRoggiType
{
    public required RoggiTypeInfo[] ArgTypes { get; init; }
}