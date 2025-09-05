namespace SixShaded.FZOTypeMatch;

public class KorssaTypeInfo : IFZOTypeInfo<IKorssaType>
{
    public required Type Origin { get; init; }
    public required RoggiTypeInfo[] ArgTypes { get; init; }
    public required RoggiTypeInfo OutputType { get; init; }
    public required IOption<IKorssaType> MatchedType { get; init; }
}