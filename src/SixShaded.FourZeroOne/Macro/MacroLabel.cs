namespace SixShaded.FourZeroOne.Macro;

public sealed record MacroLabel
{
    private readonly string _identifier;
    private readonly string _package;
    public required string Package { get => _package; init => _package = value.ToLower(); }
    public required string Identifier { get => _identifier; init => _identifier = value.ToLower(); }
    public override string ToString() => $"{Package}~{Identifier}";
}