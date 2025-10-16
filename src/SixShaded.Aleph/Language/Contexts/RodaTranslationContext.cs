namespace SixShaded.Aleph.Language.Contexts;
using FZOTypeMatch;
using Segments;
using FourZeroOne.Roggi.Unsafe;

public class RodaTranslationContext
{
    internal RodaTranslationContext(Addr source)
    {
        Roda = source;
    }

    public Addr Roda { get; }
}