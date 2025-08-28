namespace SixShaded.FZOTypeMatch;

public class KorssaTypeInfo : IFZOTypeInfo<IKorssaType>
{
    public required Type Origin { get; init; }
    public required IOption<IKorssaType> MatchedType { get; init; }
}