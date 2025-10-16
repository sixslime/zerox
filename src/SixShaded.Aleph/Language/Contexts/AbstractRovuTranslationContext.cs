namespace SixShaded.Aleph.Language.Contexts;

using FZOTypeMatch;
using Segments;
using FourZeroOne.Roveggi.Unsafe;

public class AbstractRovuTranslationContext
{
    internal AbstractRovuTranslationContext(IAbstractRovu source)
    {
        AbstractRovu = source;
    }

    public IAbstractRovu AbstractRovu { get; }
}