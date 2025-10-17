namespace SixShaded.Aleph;

public record RunArgs
{
    public required Language.ILanguageKey LanguageKey { get; init; }
    public required IProcessorFZO Processor { get; init; }
}