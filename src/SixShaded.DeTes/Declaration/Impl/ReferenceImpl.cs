namespace SixShaded.DeTes.Declaration.Impl;

internal class ReferenceImpl<R> : IReferenceAccessor, IDeTesReference<R> where R : class, Rog
{
    private IOption<IMemoryFZO> _memory = new None<IMemoryFZO>();
    private IOption<IOption<R>> _roggi = new None<IOption<R>>();
    private IOption<IKorssa<R>> _korssaOverride = new None<IKorssa<R>>();
    public required IKorssa<R> LinkedKorssa { get; init; }

    IKorssa<R> IDeTesReference<R>.Korssa => _korssaOverride.Check(out var v) ? v : LinkedKorssa;

    R IDeTesReference<R>.Roggi =>
        _roggi.Check(out var v) ? v.DeTesUnwrap() : throw MakeUnevaluatedException();

    IOption<R> IDeTesReference<R>.RoggiUnstable =>
        _roggi.Check(out var v) ? v : throw MakeUnevaluatedException();

    IMemoryFZO IDeTesReference<R>.Memory =>
        _memory.Check(out var v) ? v : throw MakeUnevaluatedException();

    public required string? Description { get; init; }
    Kor IKorssaLinked.LinkedKorssa => LinkedKorssa;

    void IReferenceAccessor.SetRoggi(RogOpt roggi)
    {
        if (roggi is not IOption<R> r) throw new UnexpectedRoggiTypeException(roggi.GetType(), typeof(IOption<R>));
        _roggi = r.AsSome();
    }

    void IReferenceAccessor.SetMemory(IMemoryFZO memory) => _memory = memory.AsSome();

    void IReferenceAccessor.Reset()
    {
        _roggi = _roggi.None();
        _memory = _memory.None();
        _korssaOverride = _korssaOverride.None();
    }

    void IReferenceAccessor.SetKorssa(Kor korssa) => _korssaOverride = ((IKorssa<R>)korssa).AsSome();

    private DeTesInvalidTestException MakeUnevaluatedException() =>
        new()
        {
            Value = new EDeTesInvalidTest.ReferenceUsedBeforeEvaluated
            {
                Description = Description,
                NearKorssa = LinkedKorssa,
            },
        };
}