namespace SixShaded.Aleph.ICLI.Config;

internal record SelectionKeys
{
    public required IPSequence<string> Keys { get; init; }
    public required AlephKeyPress Submit { get; init; }
    public required AlephKeyPress Cancel { get; init; }
}