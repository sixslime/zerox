namespace SixShaded.Aleph.ICLI;

internal record InputAction
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required Action<IProgramActions> ActionFunction { get; init; }
}