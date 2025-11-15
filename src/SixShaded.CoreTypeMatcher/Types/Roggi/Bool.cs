namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record Bool : IRoggiType
{
    public required Func<Rog, bool> ValueGetter { get; init; }

}