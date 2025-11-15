namespace SixShaded.Aleph;

public record AlephArgs
{
    public required Language.ILanguageKey LanguageKey { get; init; }
    public required IProcessorFZO Processor { get; init; }
}