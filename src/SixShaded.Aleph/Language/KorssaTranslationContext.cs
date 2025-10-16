namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
using Segments;

public class KorssaTranslationContext
{
    private int _currentArgIndex = 0;
    internal KorssaTranslationContext(Kor source)
    {
        Korssa = source;
    }

    public Kor Korssa { get; }
    public ITranslationMarker NextArg()
    {
        if (_currentArgIndex >= Korssa.ArgKorssas.Length)
            throw new Exception("NextArg() called when Korssa did not have another arg.");
        return new KorssaArgSegment()
        {
            Index = _currentArgIndex++
        };
    }
}