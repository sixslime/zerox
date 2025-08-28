namespace SixShaded.FZOTypeMatch;

public class RovetuTypeInfo : IFZOTypeInfo<IRovetuType>
{
    internal RovetuTypeInfo(Type origin, IOption<IRovetuType> match)
    {
        Origin = origin;
        MatchedType = match;
    }

    public Type Origin { get; }
    public IOption<IRovetuType> MatchedType { get; }
}