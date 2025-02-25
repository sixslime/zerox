namespace SixShaded.FourZeroOne;

public sealed class Axodu
{
    private string _name { get; init; } = null!;
    public required string Name { get => _name; init => _name = value.ToLower(); }
}