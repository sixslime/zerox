namespace SixShaded.FZOTypeMatch;

public record RovetuTypeInfo : IFZOTypeInfo<IRovetuType>
{
    public required IOption<RoggiTypeInfo> MemoryDataType { get; init; }
    public required Type Origin { get; init; }
    public required IOption<IRovetuType> MatchedType { get; init; }
}