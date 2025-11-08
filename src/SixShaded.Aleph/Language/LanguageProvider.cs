namespace SixShaded.Aleph.Language;

using Contexts;
using Segments;
using SixShaded.FourZeroOne.Roveggi.Unsafe;

internal class LanguageProvider(ILanguageKey key)
{
    public ILanguageKey LanguageKey { get; } = key;

    public Translation TranslateKorssa(Kor val) =>
        LanguageKey.TranslateKorssa(new(val), new())
            .OrElse(
            () =>
            {
                var context = new KorssaTranslationContext(val);
                var builder =
                    new TranslationBuilder()
                        .Text(val.GetType().Name.Split('`', 2)[0] + "(");
                for (int i = 0; i < val.ArgKorssas.Length; i++)
                    builder.Marker(context.NextArg());
                builder.Text(")");
                return builder.Build();
            });

    public Translation TranslateRoggi(Rog val) =>
        LanguageKey.TranslateRoggi(new(val), new())
            .OrElse(
            () =>
            {
                var context = new RoggiTranslationContext(val);
                return new TranslationBuilder()
                    .Text(val.ToString()!)
                    .Build();
            });
    public Translation TranslateRoda(Addr val) =>
        LanguageKey.TranslateRoda(new(val), new())
            .OrElse(
            () =>
            {
                var context = new RodaTranslationContext(val);
                return new TranslationBuilder()
                    .Text(val.ToString()!)
                    .Build();
            });
    public Translation TranslateRovu(IRovu val) =>
        LanguageKey.TranslateRovu(new(val), new())
            .OrElse(
            () =>
            {
                var context = new RovuTranslationContext(val);
                return new TranslationBuilder()
                    .Text(val.ToString()!)
                    .Build();
            });
    public Translation TranslateVarovu(IVarovu val) =>
        LanguageKey.TranslateVarovu(new(val), new())
            .OrElse(
            () =>
            {
                var context = new VarovuTranslationContext(val);
                return new TranslationBuilder()
                    .Text(val.ToString()!)
                    .Build();
            });
    public Translation TranslateAbstractRovu(IAbstractRovu val) =>
        LanguageKey.TranslateAbstractRovu(new(val), new())
            .OrElse(
            () =>
            {
                var context = new AbstractRovuTranslationContext(val);
                return new TranslationBuilder()
                    .Text(val.ToString()!)
                    .Build();
            });

    public string TranslateNolla() => LanguageKey.TranslateNolla().Or("\u2205");
}