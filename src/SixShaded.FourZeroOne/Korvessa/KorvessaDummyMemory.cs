﻿namespace SixShaded.FourZeroOne.Korvessa;

using System.Diagnostics;
using FZOSpec;

internal class KorvessaDummyMemory : IMemory
{
    public static readonly KorvessaDummyMemory INSTANCE = new();

    private KorvessaDummyMemory()
    { }

    private static Exception _useException => new UnreachableException("KorvessaDummyMemory object used (should never happen)");
    public IMemoryFZO InternalValue => throw _useException;
    public IEnumerable<ITiple<Addr, Rog>> Objects => throw _useException;
    public IEnumerable<Mel> Mellsanos => throw _useException;

    public IEnumerable<ITiple<IRoveggi<D>, R>> GetRovedanggiAssignmentsOfType<D, R>()
        where D : Rovedantu<R>
        where R : class, Rog =>
        throw _useException;

    public IOption<R> GetObject<R>(IRoda<R> address)
        where R : class, Rog =>
        throw _useException;

    public RogOpt GetObjectUnsafe(Addr address) => throw _useException;
    public IMemory WithMellsanos(IEnumerable<Mel> mellsanos) => throw _useException;

    public IMemory WithObjects<R>(IEnumerable<ITiple<IRoda<R>, R>> insertions)
        where R : class, Rog =>
        throw _useException;

    public IMemory WithObjectsUnsafe(IEnumerable<ITiple<Addr, Rog>> insertions) => throw _useException;
    public IMemory WithClearedAddresses(IEnumerable<Addr> removals) => throw _useException;
}