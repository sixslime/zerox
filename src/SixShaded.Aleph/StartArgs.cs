namespace SixShaded.Aleph;

public record StartArgs
{
    public required Language.LanguageProvider LanguageProvider { get; init; }
}