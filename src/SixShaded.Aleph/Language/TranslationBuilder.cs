namespace SixShaded.Aleph.Language;

using FZOTypeMatch;
using Segments;
using FourZeroOne.Roggi.Unsafe;
using FourZeroOne.Roveggi.Unsafe;

public class TranslationBuilder
{
    private readonly List<ITranslationSegment> _segments = [];

    internal TranslationBuilder()
    { }

    public Translation Build() => new(_segments);
    public IOption<Translation> BuildAsSome() => Build().AsSome();

    public TranslationBuilder Text(string text) =>
        AddSegment(
        new TextSegment()
        {
            Text = text
        });

    public TranslationBuilder InlineTranslation(Kor value) =>
        AddSegment(
        new InlineKorssaSegment
        {
            Value = value,
        });

    public TranslationBuilder InlineTranslation(RogOpt value) =>
        AddSegment(
        new InlineRoggiSegment
        {
            Value = value,
        });

    public TranslationBuilder InlineTranslation(Addr value) =>
        AddSegment(
        new InlineRodaSegment
        {
            Value = value,
        });

    public TranslationBuilder InlineTranslation(IRovu value) =>
        AddSegment(
        new InlineRovuSegment
        {
            Value = value,
        });

    public TranslationBuilder InlineTranslation(IVarovu value) =>
        AddSegment(
        new InlineVarovuSegment
        {
            Value = value,
        });

    public TranslationBuilder InlineTranslation(IAbstractRovu value) =>
        AddSegment(
        new InlineAbstractRovuSegment
        {
            Value = value,
        });
    public TranslationBuilder InlineTranslation(KorssaTypeInfo value) =>
        AddSegment(
        new InlineKorssaTypeInfoSegment()
        {
            Value = value,
        });
    public TranslationBuilder InlineTranslation(RovetuTypeInfo value) =>
        AddSegment(
        new InlineRovetuTypeInfoSegment()
        {
            Value = value,
        });
    public TranslationBuilder InlineTranslation(RoggiTypeInfo value) =>
        AddSegment(
        new InlineRoggiTypeInfoSegment()
        {
            Value = value,
        });
    public TranslationBuilder Marker(ITranslationMarker marker) => AddSegment(marker);

    private TranslationBuilder AddSegment(ITranslationSegment segment)
    {
        _segments.Add(segment);
        return this;
    }
}