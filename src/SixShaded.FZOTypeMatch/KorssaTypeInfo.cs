namespace SixShaded.FZOTypeMatch;

public class KorssaTypeInfo : IFZOTypeInfo<IKorssaType>
{
    internal KorssaTypeInfo(Type origin, IOption<IKorssaType> match)
    {
        Origin = origin;
        MatchedType = match;
    }

    public Type Origin { get; }
    public IOption<IKorssaType> MatchedType { get; }
}