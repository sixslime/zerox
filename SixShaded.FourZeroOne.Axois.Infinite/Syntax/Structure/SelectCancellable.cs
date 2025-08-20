namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax.Structure;

public record SelectCancellableDirect<RIn, ROut>
    where RIn : class, Rog
    where ROut : class, Rog
{
    public required IKorssa<MetaFunction<RIn, ROut>> Select { get; init; }
    public required IKorssa<MetaFunction<ROut>> Cancel { get; init; }
}