namespace SixShaded.FourZeroOne.Korvessa;

using System.Diagnostics;
using FZOSpec;

internal class KorvessaDummyMemory : IMemory
{
    public static readonly KorvessaDummyMemory INSTANCE = new();

    private KorvessaDummyMemory()
    { }

    private static Exception _useException => new UnreachableException("KorvessaDummyMemory object used (should never happen)");
    public IMemoryFZO InternalValue => throw _useException;
    public IEnumerable<ITiple<IRoda<>, Rog>> Objects => throw _useException;
    public IEnumerable<Mel> Mellsanos => throw _useException;

    public IOption<R> GetObject<R>(IRoda<R> address)
        where R : class, Rog =>
        throw _useException;

    public RogOpt GetObjectUnsafe(IRoda<> address) => throw _useException;
    public IMemory WithMellsanos(IEnumerable<Mel> mellsanos) => throw _useException;

    public IMemory WithObjects<R>(IEnumerable<ITiple<IRoda<R>, R>> insertions)
        where R : class, Rog =>
        throw _useException;

    public IMemory WithObjectsUnsafe(IEnumerable<ITiple<IRoda<>, Rog>> insertions) => throw _useException;
    public IMemory WithClearedAddresses(IEnumerable<IRoda<>> removals) => throw _useException;
}