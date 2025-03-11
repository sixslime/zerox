namespace SixShaded.FourZeroOne.Core.Syntax.Structure.Korssa;

public sealed record SubEnvironment<R>
    where R : class, Rog
{
    public required List<Kor> Environment { get; init; }
    public required IKorssa<R> Value { get; init; }
}