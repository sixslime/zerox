namespace SixShaded.FZOTypeMatch;

public record KorssaTypeInfo : IFZOTypeInfo<IKorssaType>
{
    public required EArgTypes ArgTypes { get; init; }
    public required RoggiTypeInfo ResultType { get; init; }
    public required Type Origin { get; init; }
    public required IOption<IKorssaType> Match { get; init; }

    public abstract record EArgTypes
    {
        public record Positional : EArgTypes
        {
            public required RoggiTypeInfo[] Types { get; init; }
        }

        public record Combiner : EArgTypes
        {
            public required RoggiTypeInfo Type { get; init; }
        }
    }
}