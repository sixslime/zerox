namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
public class KorssaTranslationContext
{
    internal KorssaTranslationContext(Kor source)
    {
        _source = source;
    }
    private Kor _source;
    public int ArgCount => _source.ArgKorssas.Length;
    public ITranslationMarker NextArg()
    {
        throw new NotImplementedException();
    }
}