namespace SixShaded.Aleph.Language.Contexts;

using FZOTypeMatch;
using Segments;
using FourZeroOne.Roveggi.Unsafe;

public class RovuTranslationContext
{
    internal RovuTranslationContext(IRovu source)
    {
        Rovu = source;
    }

    public IRovu Rovu { get; }
}