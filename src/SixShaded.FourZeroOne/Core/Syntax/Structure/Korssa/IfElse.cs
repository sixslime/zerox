namespace SixShaded.FourZeroOne.Core.Syntax.Structure.Korssa;

public sealed record IfElse<R>
    where R : class, Rog
{
    public required IKorssa<R> Then { get; init; }
    public required IKorssa<R> Else { get; init; }
}