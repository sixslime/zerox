namespace SixShaded.Aleph.Language;
using Contexts;
using FZOTypeMatch;
using TranslationBuilders;

// LEFTOFF:
// need way to recursively translate; e.g. for metafunction display
// probably need special return type
// builder pattern, similar to StringBuilder
public interface ILanguageKey
{
    public IOption<Translation> TranslateKorssa(KorssaTranslationContext context, TranslationBuilder builder);
    public IOption<Translation> TranslateRoggi(RoggiTranslationContext context, TranslationBuilder builder);
    public IOption<Translation> TranslateRoda(RodaTranslationContext context, TranslationBuilder builder);
    public IOption<Translation> TranslateRovu(RovuTranslationContext context, TranslationBuilder builder);
    public IOption<Translation> TranslateVarovu(VarovuTranslationContext context, TranslationBuilder builder);
    public IOption<Translation> TranslateAbstractRovu(AbstractRovuTranslationContext context, TranslationBuilder builder);
    public string TranslateNolla();
}