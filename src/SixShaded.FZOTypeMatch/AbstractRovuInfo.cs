namespace SixShaded.FZOTypeMatch;
using FourZeroOne.Roveggi.Unsafe;

public record AbstractRovuInfo
{
    public required IAbstractRovu Rovu { get; init; }
    public required RoggiTypeInfo DataType { get; init; }
    public required RovetuTypeInfo RovetuType { get; init; }
    public required EInteraction Interaction { get; init; }
    public enum EInteraction
    {
        Get,
        Set
    }
}