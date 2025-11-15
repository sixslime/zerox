namespace SixShaded.Aleph.Language.Segments;

internal class InlineRodaSegment : ITranslationSegment
{
    public required Addr Value { get; init; }
}