namespace SixShaded.FZOTypeMatch;

using FourZeroOne.Roveggi.Unsafe;

public record VarovuInfo
{
    public required IVarovu Varovu { get; init; }
    public required RovetuTypeInfo RovetuType { get; init; }
    public required RoggiTypeInfo DataType { get; init; }
    public required RoggiTypeInfo KeyType { get; init; }
}