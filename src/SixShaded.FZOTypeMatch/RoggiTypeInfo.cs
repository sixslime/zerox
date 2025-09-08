namespace SixShaded.FZOTypeMatch;

public record RoggiTypeInfo : IFZOTypeInfo<IRoggiType>
{
    public required Type Origin { get; init; }
    public required IOption<IRoggiType> Match { get; init; }
}