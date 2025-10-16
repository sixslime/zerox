namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
using CoreTypeMatcher;
public record LanguageProvider
{
    public required ILanguageKey Key { get; init; }
}