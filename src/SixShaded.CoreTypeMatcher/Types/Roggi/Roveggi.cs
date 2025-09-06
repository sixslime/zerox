namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record Roveggi : IRoggiType
{
    public required RovetuTypeInfo RovetuType { get; init; }

}