namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
using Segments;
using FourZeroOne.Roggi.Unsafe;
using FourZeroOne.Roveggi.Unsafe;
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
    public TranslationBuilder InlineTranslation(Kor value) => InternalInlineTranlation(value);
    public TranslationBuilder InlineTranslation(Rog value) => InternalInlineTranlation(value);
    public TranslationBuilder InlineTranslation(Addr value) => InternalInlineTranlation(value);
    public TranslationBuilder InlineTranslation(IRovu value) => InternalInlineTranlation(value);
    public TranslationBuilder InlineTranslation(IVarovu value) => InternalInlineTranlation(value);
    public TranslationBuilder InlineTranslation(IAbstractRovu value) => InternalInlineTranlation(value);

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
}