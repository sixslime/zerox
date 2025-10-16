namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
using Segments;

public class KorssaTranslationContext
{
    private int _currentArgIndex = 0;
    private bool _restArgsRetrieved = false;
    internal KorssaTranslationContext(KorssaTypeInfo source)
    {
        TypeInfo = source;
    }

    public KorssaTypeInfo TypeInfo { get; }
    public ITranslationMarker NextArg()
    {
        if (_restArgsRetrieved)
            throw new Exception("Cannot call NextArg() when RestArgs() was already called.");
        return new KorssaOneArgSegment()
        {
            Index = _currentArgIndex++
        };
    }

    public ITranslationMarker RestArgs(string seperator)
    {
        if (_restArgsRetrieved)
            throw new Exception("Cannot call RestArgs() when RestArgs() was already called.");
        _restArgsRetrieved = true;
        return new KorssaRestArgsSegment()
        {
            Seperator = seperator,
            StartIndex = _currentArgIndex
        };
    }

    public ITranslationResolution Resolve(Func<Kor, string> expression)
    {
        return new ResolutionSegment<Kor>()
        {
            Expression = expression
        };
    }
}