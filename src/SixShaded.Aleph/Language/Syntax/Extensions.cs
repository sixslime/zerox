namespace SixShaded.Aleph.Language.Syntax;

internal static class Extensions
{
    public static LanguageProvider AsProvider(this ILanguageKey key) => new(key);
}