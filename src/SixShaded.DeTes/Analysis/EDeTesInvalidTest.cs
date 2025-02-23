namespace SixShaded.DeTes.Analysis;

public abstract record EDeTesInvalidTest
{
    public sealed record ReferenceUsedBeforeEvaluated : EDeTesInvalidTest
    {
        public required Tok NearToken { get; init; }
        public required string? Description { get; init; }
    }

    public sealed record EmptyDomain : EDeTesInvalidTest
    {
        public required Tok NearToken { get; init; }
        public required string? Description { get; init; }
    }

    public sealed record NoSelectionDomainDefined : EDeTesInvalidTest
    {
        public required Tok SelectionToken { get; init; }
    }

    public sealed record DomainUsedOutsideOfScope : EDeTesInvalidTest
    {
        public required Tok NearToken { get; init; }
        public required string? Description { get; init; }
        public required int[][] Domain { get; init; }
    }

    public sealed record InvalidDomainSelection : EDeTesInvalidTest
    {
        public required Tok NearToken { get; init; }
        public required string? Description { get; init; }
        public required Tok SelectionToken { get; init; }
        public required int[][] Domain { get; init; }
        public required int[] InvalidSelection { get; init; }
        public required int ExpectedSelectionSize { get; init; }
        public required int ExpectedMaxIndex { get; init; }
    }
}