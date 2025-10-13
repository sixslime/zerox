namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
using Segments;
public class TranslationBuilder
{
    private List<ITranslationSegment> _segments = [];

    internal TranslationBuilder()
    {

    }
    public Translation Build()
    {
        throw new NotImplementedException();
    }

    public IOption<Translation> BuildAsSome() => Build().AsSome();
    public TranslationBuilder Text(string text)
    {
        _segments.Add(
        new TextSegment
        {
            Text = text
        });
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
    public TranslationBuilder InlineTranslation(RovuInfo value)
    {
        return this;
    }
    public TranslationBuilder InlineTranslation(VarovuInfo value)
    {
        return this;
    }
    public TranslationBuilder InlineTranslation(AbstractRovuInfo value)
    {
        return this;
    }

    public TranslationBuilder Marker(ITranslationMarker marker)
    {
        _segments.Add(marker);
        return this;
    }
    public TranslationBuilder Resolution(ITranslationResolution resolution)
    {
        _segments.Add(resolution);
        return this;
    }
}