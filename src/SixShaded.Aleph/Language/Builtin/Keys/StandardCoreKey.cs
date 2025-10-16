namespace SixShaded.Aleph.Language.Builtin.Keys;

using Contexts;
using FourZeroOne.Roveggi.Unsafe;
using FZOTypeMatch;

public class StandardCoreKey : ILanguageKey
{
    public IOption<Translation> TranslateKorssa(KorssaTranslationContext context, TranslationBuilder builder) => new None<Translation>();
    public IOption<Translation> TranslateRoggi(RoggiTranslationContext context, TranslationBuilder builder) => new None<Translation>();
    public IOption<Translation> TranslateRoda(RodaTranslationContext context, TranslationBuilder builder) => new None<Translation>();
    public IOption<Translation> TranslateRovu(RovuTranslationContext context, TranslationBuilder builder) => new None<Translation>();
    public IOption<Translation> TranslateVarovu(VarovuTranslationContext context, TranslationBuilder builder) => new None<Translation>();
    public IOption<Translation> TranslateAbstractRovu(AbstractRovuTranslationContext context, TranslationBuilder builder) => new None<Translation>();
    public IOption<string> TranslateNolla() => new None<string>();
}