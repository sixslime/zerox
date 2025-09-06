namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record Multi : IRoggiType
{
    public required RoggiTypeInfo ElementType { get; init; }
}