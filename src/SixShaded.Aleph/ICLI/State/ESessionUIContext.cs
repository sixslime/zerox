namespace SixShaded.Aleph.ICLI.State;

internal abstract record ESessionUIContext
{
    public sealed record TopLevel : ESessionUIContext;

    public sealed record SelectingProgressor : ESessionUIContext
    {
        public required bool Backward { get; init; }
    }
}