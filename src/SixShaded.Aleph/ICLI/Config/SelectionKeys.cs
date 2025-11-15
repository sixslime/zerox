namespace SixShaded.Aleph.ICLI.Config;

internal record SelectionKeys
{
    public required string Indicators { get; init; }
    public required AlephKeyPress Submit { get; init; }
    public required AlephKeyPress Cancel { get; init; }
    public required AlephKeyPress ShowAvailable { get; init; }
    public required AlephKeyPress Delete { get; init; }
}