namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax.Structure;

public record SelectCancellable<RIn, ROut>
    where RIn : class, Rog
    where ROut : class, Rog
{
    public required MetaDefinition<RIn, ROut> Select { get; init; }
    public required MetaDefinition< ROut> Cancel { get; init; }
}