namespace SixShaded.FZOTypeMatch;

public class RoggiTypeInfo : IFZOTypeInfo<IRoggiType>
{
    internal RoggiTypeInfo(Type origin, IOption<IRoggiType> match)
    {
        Origin = origin;
        MatchedType = match;
    }

    public Type Origin { get; }
    public IOption<IRoggiType> MatchedType { get; }
}