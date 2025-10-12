namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
using CoreTypeMatcher;
public record LanguageProvider
{
    public required FZOTypeMatch TypeMatch { get; init; }
    public required ILanguageKey Key { get; init; }
}