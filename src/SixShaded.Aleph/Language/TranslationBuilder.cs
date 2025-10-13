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
    public TranslationBuilder InlineTranslation(KorssaTypeInfo value) => InternalInlineTranlation(value);
    public TranslationBuilder InlineTranslation(RoggiTypeInfo value) => InternalInlineTranlation(value);
    public TranslationBuilder InlineTranslation(RovetuTypeInfo value) => InternalInlineTranlation(value);
    public TranslationBuilder InlineTranslation(RodaInfo value) => InternalInlineTranlation(value);
    public TranslationBuilder InlineTranslation(RovuInfo value) => InternalInlineTranlation(value);
    public TranslationBuilder InlineTranslation(VarovuInfo value) => InternalInlineTranlation(value);
    public TranslationBuilder InlineTranslation(AbstractRovuInfo value) => InternalInlineTranlation(value);

    private TranslationBuilder InternalInlineTranlation(object value)
    {
        _segments.Add(
        new InlineTranslationSegment()
        {
            Value = value
        });
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