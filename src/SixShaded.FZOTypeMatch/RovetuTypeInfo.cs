namespace SixShaded.FZOTypeMatch;

public record RovetuTypeInfo : IFZOTypeInfo<IRovetuType>
{
    public required Type Origin { get; init; }
    public required IOption<IRovetuType> MatchedType { get; init; }
}