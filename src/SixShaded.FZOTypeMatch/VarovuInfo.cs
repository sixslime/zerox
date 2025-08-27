namespace SixShaded.FZOTypeMatch;
using FourZeroOne.Roveggi.Unsafe;

public class VarovuInfo
{
    internal VarovuInfo(IVarovu varovu)
    {
        Varovu = varovu;
        // TODO
    }

    public enum EInteractionType
    {
        Get,
        Set
    }

    public IVarovu Varovu { get; }
    public IRoggiType KeyType { get; }
    public IRoggiType DataType { get; }
    public IRovetuType RovetuType { get; }
}