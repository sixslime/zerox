namespace SixShaded.Aleph.ICLI;

internal record InputAction
{
    public static InputAction Exit { get; } =
        new()
        {
            Name = "exit",
            Description = "Exit program.",
            ActionFunction = actions => actions.Exit(),
        };
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required Action<IProgramActions> ActionFunction { get; init; }
}