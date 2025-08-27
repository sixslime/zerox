namespace SixShaded.FZOTypeMatch;
using FourZeroOne.Roveggi.Unsafe;

public class AbstractRovuInfo
{
    internal AbstractRovuInfo(IAbstractRovu rovu)
    {
        Rovu = rovu;
        // TODO
    }

    public enum EInteractionType
    {
        Get,
        Set
    }

    public EInteractionType InteractionType { get; }
    public IAbstractRovu Rovu { get; }
    public IRoggiType DataType { get; }
    public IRovetuType RovetuType { get; }
}