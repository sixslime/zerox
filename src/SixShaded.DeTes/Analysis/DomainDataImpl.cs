namespace SixShaded.DeTes.Analysis;

internal class DomainDataImpl : IDeTesDomainData
{
    public required string? Description { get; init; }
    public required int[][] Values { get; init; }
}