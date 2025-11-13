namespace SixShaded.Aleph.Language.Segments;
using FZOTypeMatch;
internal class InlineRoggiTypeInfoSegment : ITranslationSegment
{
    public required RoggiTypeInfo Value { get; init; }
}