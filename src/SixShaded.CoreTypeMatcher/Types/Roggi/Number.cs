namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record Number : IRoggiType
{
    public required Func<Rog, int> ValueGetter { get; init; }
}