namespace SixShaded.Aleph.Language.Contexts;

using FZOTypeMatch;
using Segments;

public class KorssaTranslationContext
{
    private int _currentArgIndex;

    internal KorssaTranslationContext(Kor source)
    {
        Korssa = source;
    }

    public Kor Korssa { get; }

    public ITranslationMarker NextArg()
    {
        if (_currentArgIndex >= Korssa.ArgKorssas.Length)
            throw new("NextArg() called when Korssa did not have another arg.");
        return new KorssaArgSegment
        {
            Index = _currentArgIndex++,
        };
    }
}