namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
using TranslationBuilders;

// LEFTOFF:
// need way to recursively translate; e.g. for metafunction display
// probably need special return type
// builder pattern, similar to StringBuilder
public interface ILanguageKey
{
    public IOption<Translation> TranslateKorssa(KorssaTranslationContext context, TranslationBuilder builder);
    public IOption<Translation> TranslateRoggi(TranslationBuilder builder, object context);
    public IOption<Translation> TranslateRovetu(TranslationBuilder builder, object context);
    public IOption<Translation> TranslateRoda(TranslationBuilder builder, object context);
    public IOption<Translation> TranslateRovu(TranslationBuilder builder, object context);
    public IOption<Translation> TranslateVarovu(TranslationBuilder builder, object context);
    public IOption<Translation> TranslateAbstractRovu(TranslationBuilder builder, object context);
    public string TranslateNolla();
}