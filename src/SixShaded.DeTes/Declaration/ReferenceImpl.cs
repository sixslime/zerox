namespace SixShaded.DeTes.Declaration;

internal class ReferenceImpl<R> : IReferenceAccessor, IDeTesReference<R> where R : class, Res
{
    private IOption<IMemoryFZO> _memory = new None<IMemoryFZO>();
    private IOption<IOption<R>> _resolution = new None<IOption<R>>();
    private IOption<IToken<R>> _tokenOverride = new None<IToken<R>>();
    public required IToken<R> LinkedToken { get; init; }

    IToken<R> IDeTesReference<R>.Tok => _tokenOverride.Check(out var v) ? v : LinkedToken;

    R IDeTesReference<R>.Resolution =>
        _resolution.Check(out var v) ? v.DeTesUnwrap() : throw MakeUnevaluatedException();

    IOption<R> IDeTesReference<R>.ResolutionUnstable =>
        _resolution.Check(out var v) ? v : throw MakeUnevaluatedException();

    IMemoryFZO IDeTesReference<R>.Memory =>
        _memory.Check(out var v) ? v : throw MakeUnevaluatedException();

    public required string? Description { get; init; }
    Tok ITokenLinked.LinkedToken => LinkedToken;

    void IReferenceAccessor.SetResolution(ResOpt resolution)
    {
        if (resolution is not IOption<R> r) throw new UnexpectedResolutionTypeException(resolution.GetType(), typeof(IOption<R>));
        _resolution = r.AsSome();
    }

    void IReferenceAccessor.SetMemory(IMemoryFZO memory) => _memory = memory.AsSome();

    void IReferenceAccessor.Reset()
    {
        _resolution = _resolution.None();
        _memory = _memory.None();
        _tokenOverride = _tokenOverride.None();
    }

    void IReferenceAccessor.SetToken(Tok token) => _tokenOverride = ((IToken<R>)token).AsSome();

    private DeTesInvalidTestException MakeUnevaluatedException() =>
        new()
        {
            Value = new EDeTesInvalidTest.ReferenceUsedBeforeEvaluated
            {
                Description = Description,
                NearToken = LinkedToken,
            },
        };
}