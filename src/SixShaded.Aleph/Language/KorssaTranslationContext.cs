namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
public class KorssaTranslationContext
{
    internal KorssaTranslationContext(KorssaTypeInfo source)
    {
        TypeInfo = source;
    }

    public KorssaTypeInfo TypeInfo { get; }
    public ITranslationMarker NextArg()
    {
        throw new NotImplementedException();
    }

    public ITranslationMarker RestArgs(string seperator)
    {
        throw new NotImplementedException();
    }

    public ITranslationResolution Resolve(Func<Kor, string> expression)
    {
        throw new NotImplementedException(); 
    }
}