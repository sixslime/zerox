namespace SixShaded.Aleph.Language.Segments;

internal class InlineVarovuSegment : ITranslationSegment
{
    public required FourZeroOne.Roveggi.Unsafe.IVarovu Value { get; init; }
}