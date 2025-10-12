namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
public class TranslationBuilder
{
    public TranslationBuilder Text(string text)
    {
        return this;
    }
    public TranslationBuilder InlineTranslation(KorssaTypeInfo value)
    {
        return this;
    }
    public TranslationBuilder InlineTranslation(RoggiTypeInfo value)
    {
        return this;
    }
    public TranslationBuilder InlineTranslation(RovetuTypeInfo value)
    {
        return this;
    }
    public TranslationBuilder InlineTranslation(RodaInfo value)
    {
        return this;
    }
    public TranslationBuilder InlineTranslation(VarovuInfo value)
    {
        return this;
    }
    public TranslationBuilder InlineTranslation(RovuInfo value)
    {
        return this;
    }

    public TranslationBuilder Marker(ITranslationMarker marker)
    {
        return this;
    }
}