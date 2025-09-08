namespace SixShaded.FZOTypeMatch;

public record KorssaTypeInfo : IFZOTypeInfo<IKorssaType>
{
    public required RoggiTypeInfo[] ArgTypes { get; init; }
    public required RoggiTypeInfo ResultType { get; init; }
    public required Type Origin { get; init; }
    public required IOption<IKorssaType> Match { get; init; }
}