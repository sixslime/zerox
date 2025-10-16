namespace SixShaded.Aleph.Language.Builtin.Keys;

using Contexts;

public class MergedKey(ILanguageKey primary, ILanguageKey fallback) : ILanguageKey
{
    public ILanguageKey Primary { get; } = primary;
    public ILanguageKey Fallback { get; } = fallback;
    public IOption<Translation> TranslateKorssa(KorssaTranslationContext context, TranslationBuilder builder) => Primary.TranslateKorssa(context, builder).OrElseTry(() => Fallback.TranslateKorssa(context, builder));
    public IOption<Translation> TranslateRoggi(RoggiTranslationContext context, TranslationBuilder builder) => Primary.TranslateRoggi(context, builder).OrElseTry(() => Fallback.TranslateRoggi(context, builder));
    public IOption<Translation> TranslateRoda(RodaTranslationContext context, TranslationBuilder builder) => Primary.TranslateRoda(context, builder).OrElseTry(() => Fallback.TranslateRoda(context, builder));
    public IOption<Translation> TranslateRovu(RovuTranslationContext context, TranslationBuilder builder) => Primary.TranslateRovu(context, builder).OrElseTry(() => Fallback.TranslateRovu(context, builder));
    public IOption<Translation> TranslateVarovu(VarovuTranslationContext context, TranslationBuilder builder) => Primary.TranslateVarovu(context, builder).OrElseTry(() => Fallback.TranslateVarovu(context, builder));
    public IOption<Translation> TranslateAbstractRovu(AbstractRovuTranslationContext context, TranslationBuilder builder) => Primary.TranslateAbstractRovu(context, builder).OrElseTry(() => Fallback.TranslateAbstractRovu(context, builder));
    public IOption<string> TranslateNolla() => Primary.TranslateNolla().OrElseTry(Fallback.TranslateNolla);
}