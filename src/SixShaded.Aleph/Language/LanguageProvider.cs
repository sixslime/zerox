namespace SixShaded.Aleph.Language;

using Contexts;
using SixShaded.FourZeroOne.Roveggi.Unsafe;

internal class LanguageProvider(ILanguageKey key)
{
    public ILanguageKey LanguageKey { get; } = key;

    public IOption<Translation> TranslateKorssa(Kor val) => LanguageKey.TranslateKorssa(new(val), new());
    public IOption<Translation> TranslateRoggi(Rog val) => LanguageKey.TranslateRoggi(new(val), new());
    public IOption<Translation> TranslateRoda(Addr val) => LanguageKey.TranslateRoda(new(val), new());
    public IOption<Translation> TranslateRovu(IRovu val) => LanguageKey.TranslateRovu(new(val), new());
    public IOption<Translation> TranslateVarovu(IVarovu val) => LanguageKey.TranslateVarovu(new(val), new());
    public IOption<Translation> TranslateAbstractRovu(IAbstractRovu val) => LanguageKey.TranslateAbstractRovu(new(val), new());
    public IOption<string> TranslateNolla() => LanguageKey.TranslateNolla();
}