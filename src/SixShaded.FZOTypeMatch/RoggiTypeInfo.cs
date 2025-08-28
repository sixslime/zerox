namespace SixShaded.FZOTypeMatch;

public class RoggiTypeInfo : IFZOTypeInfo<IRoggiType>
{
    public required Type Origin { get; init; }
    public required IOption<IRoggiType> MatchedType { get; init; }
}