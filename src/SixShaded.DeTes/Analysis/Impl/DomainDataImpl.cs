namespace SixShaded.DeTes.Analysis.Impl;

internal class DomainDataImpl : IDeTesDomainData
{
    public required string? Description { get; init; }
    public required int[][] Values { get; init; }
}