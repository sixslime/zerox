namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record NumRange : IRoggiType
{
    public required Func<Rog, int> MinGetter { get; init; }
    public required Func<Rog, int> MaxGetter { get; init; }

}