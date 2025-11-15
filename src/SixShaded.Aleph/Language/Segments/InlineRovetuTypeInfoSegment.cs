namespace SixShaded.Aleph.Language.Segments;
using FZOTypeMatch;
internal class InlineRovetuTypeInfoSegment : ITranslationSegment
{
    public required RovetuTypeInfo Value { get; init; }
}