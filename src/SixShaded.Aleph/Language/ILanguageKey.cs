namespace SixShaded.Aleph.Language;

using Contexts;
using FZOTypeMatch;

public interface ILanguageKey
{
    public IOption<Translation> TranslateKorssa(KorssaTranslationContext context, TranslationBuilder builder);
    public IOption<Translation> TranslateRoggi(RoggiTranslationContext context, TranslationBuilder builder);
    public IOption<Translation> TranslateRoda(RodaTranslationContext context, TranslationBuilder builder);
    public IOption<Translation> TranslateRovu(RovuTranslationContext context, TranslationBuilder builder);
    public IOption<Translation> TranslateVarovu(VarovuTranslationContext context, TranslationBuilder builder);
    public IOption<Translation> TranslateAbstractRovu(AbstractRovuTranslationContext context, TranslationBuilder builder);
    public IOption<string> TranslateNolla();
}