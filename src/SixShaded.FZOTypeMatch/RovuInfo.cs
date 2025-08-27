namespace SixShaded.FZOTypeMatch;
using FourZeroOne.Roveggi.Unsafe;

public class RovuInfo
{
    internal RovuInfo(IRovu rovu)
    {
        Rovu = rovu;
        // TODO
    }

    public IRovu Rovu { get; }
    public IRoggiType DataType { get; }
    public IRovetuType RovetuType { get; }
}