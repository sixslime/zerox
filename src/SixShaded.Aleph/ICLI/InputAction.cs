namespace SixShaded.Aleph.ICLI;

internal record InputAction
{
    public static InputAction Quit { get; } =
        new()
        {
            Name = "quit",
            Description = "quit AlephICLI",
            ActionFunction = actions => actions.Quit(),
        };
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required Action<IProgramActions> ActionFunction { get; init; }
}