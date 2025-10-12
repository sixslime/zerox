namespace SixShaded.FZOTypeMatch;

public record RovetuTypeInfo : IFZOTypeInfo<IRovetuType>
{
    public required IOption<RoggiTypeInfo> MemoryDataType { get; init; }
    public required bool IsConcrete { get; init; }
    public required RovetuTypeInfo[] Inherits { get; init; }
    public required Type Origin { get; init; }
    public required IOption<IRovetuType> Match { get; init; }
}