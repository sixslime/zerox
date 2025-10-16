namespace SixShaded.Aleph.Language;
using FZOTypeMatch;
using CoreTypeMatcher;
public class LanguageProvider(ILanguageKey key)
{
    public ILanguageKey Key { get; } = key;
}