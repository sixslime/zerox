namespace SixShaded.CoreTypeMatcher.Types.Roggi;

public record NumRange : IRoggiType
{
    public required Func<Rog, FourZeroOne.Core.Roggis.Number> StartGetter { get; init; }
    public required Func<Rog, FourZeroOne.Core.Roggis.Number> EndGetter { get; init; }

}