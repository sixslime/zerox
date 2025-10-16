namespace SixShaded.Aleph;

public record RunArgs
{
    public required Language.LanguageProvider LanguageProvider { get; init; }
}