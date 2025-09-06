namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record IMulti : IRoggiType
{
    public required RoggiTypeInfo ElementType { get; init; }
}