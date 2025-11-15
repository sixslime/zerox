namespace SixShaded.Aleph.Language.Segments;
using FZOTypeMatch;
internal class InlineKorssaTypeInfoSegment : ITranslationSegment
{
    public required KorssaTypeInfo Value { get; init; }
}