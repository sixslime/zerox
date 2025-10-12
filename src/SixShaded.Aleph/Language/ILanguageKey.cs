namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
using TranslationBuilders;

// LEFTOFF:
// need way to recursively translate; e.g. for metafunction display
// probably need special return type
// builder pattern, similar to StringBuilder
public interface ILanguageKey
{
    public IOption<KorssaTranslation> TranslateKorssa(IKorssaType type, KorssaTranslationBuilder builder);
    public string TranslateRoggi(IRoggiType type);
    public string TranslateRovetu(IRovetuType type);
    public string TranslateRovu(FourZeroOne.Roveggi.Unsafe.IRovu rovu);
    public string TranslateRoda(Addr roda);
}