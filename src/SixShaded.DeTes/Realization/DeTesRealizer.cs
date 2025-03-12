namespace SixShaded.DeTes.Realization;

using Impl;

public class DeTesRealizer
{
    public Task<IResult<IDeTesResult, EDeTesInvalidTest>> Realize(IDeTesTest test, IDeTesFZOSupplier supplier) => new DeTesRealizerImpl().Realize(test, supplier);
}