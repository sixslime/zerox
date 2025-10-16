namespace SixShaded.Aleph.Language.Contexts;

using FZOTypeMatch;
using Segments;
using FourZeroOne.Roveggi.Unsafe;

public class VarovuTranslationContext
{
    internal VarovuTranslationContext(IVarovu source)
    {
        Varovu = source;
    }

    public IVarovu Varovu { get; }
}