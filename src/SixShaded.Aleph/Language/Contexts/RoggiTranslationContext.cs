namespace SixShaded.Aleph.Language.Contexts;

using FZOTypeMatch;
using Segments;

public class RoggiTranslationContext
{
    internal RoggiTranslationContext(Rog source)
    {
        Roggi = source;
    }

    public Rog Roggi { get; }
}