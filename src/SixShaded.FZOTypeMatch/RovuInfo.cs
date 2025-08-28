namespace SixShaded.FZOTypeMatch;
using FourZeroOne.Roveggi.Unsafe;

public record RovuInfo
{
    public required IRovu Rovu { get; init; }
    public required RovetuTypeInfo RovetuType { get; init; }
    public required RoggiTypeInfo DataType { get; init; }
}