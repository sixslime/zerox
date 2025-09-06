namespace SixShaded.CoreTypeMatcher.Types.Roggi.Instructions;

public record Assign : IRoggiType
{
    public required RoggiTypeInfo DataType { get; init; }
}