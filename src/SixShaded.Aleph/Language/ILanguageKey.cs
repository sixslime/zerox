namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
using TranslationBuilders;

// LEFTOFF:
// need way to recursively translate; e.g. for metafunction display
// probably need special return type
// builder pattern, similar to StringBuilder
public interface ILanguageKey
{
    public IOption<Translation> TranslateKorssa(TranslationBuilder builder, KorssaTranslationContext context);
    public IOption<Translation> TranslateRoggi(TranslationBuilder builder, object context);
    public IOption<Translation> TranslateRovetu(TranslationBuilder builder, object context);
    public string TranslateNolla();
}