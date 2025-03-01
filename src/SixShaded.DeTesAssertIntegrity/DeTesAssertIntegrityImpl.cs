namespace SixShaded.DeTesAssertIntegrity;

using DeTes.Declaration;
using FourZeroOne.Korssa;
using FourZeroOne.FZOSpec;

internal class DeTesAssertIntegrityContextProvider : IDeTesContext
{
    private int _accumulator;

    void IDeTesContext.AddAssertionMemory(Kor subject, Predicate<IMemoryFZO> assertion, string? description) =>
        _accumulator += 1;

    void IDeTesContext.AddAssertionRoggi<R>(IKorssa<R> subject, Predicate<R> assertion, string? description) =>
        _accumulator += 1;

    void IDeTesContext.AddAssertionRoggiUnstable<R>(IKorssa<R> subject, Predicate<IOption<R>> assertion,
        string? description) => _accumulator += 1;

    void IDeTesContext.AddAssertionKorssa<R>(IKorssa<R> subject, Predicate<IKorssa<R>> assertion, string? description) =>
        _accumulator += 1;

    void IDeTesContext.MakeMultiSelectionDomain(Kor subject, int[][] selections,
        out IDeTesMultiDomain domainHandle, string? description) =>
        domainHandle = new DeTesAssertIntegrityDummyObject<Rog>();

    void IDeTesContext.MakeReference<R>(IKorssa<R> subject, out IDeTesReference<R> reference, string? description) =>
        reference = new DeTesAssertIntegrityDummyObject<R>();

    void IDeTesContext.MakeSingleSelectionDomain(Kor subject, int[] selections,
        out IDeTesSingleDomain domainHandle, string? description) =>
        domainHandle = new DeTesAssertIntegrityDummyObject<Rog>();

    public DeTesAssertIntegrityContext[] GetSanityContexts(DeTesDeclaration declaration)
    {
        try
        {
            _accumulator = 0;
            _ = declaration(this);
            var o = new DeTesAssertIntegrityContext[_accumulator];
            for (int i = 0; i < _accumulator; i++)
            {
                o[i] = new(i);
            }

            return o;
        }
        catch (Exception ex)
        {
            if (ex is IKnownException) throw;
            throw new InternalDeTesAssertIntegrityException(ex);
        }
    }
}

internal class DeTesAssertIntegrityDummyObject<RNon> : IDeTesMultiDomain, IDeTesSingleDomain, IDeTesReference<RNon>
    where RNon : class, Rog
{
    public static readonly DeTesAssertIntegrityDummyObject<RNon> INSTANCE = new();
    int[] IDeTesMultiDomain.SelectedIndicies() => throw new UnexpectedDeTesUseException();
    IKorssa<RNon> IDeTesReference<RNon>.Korssa => throw new UnexpectedDeTesUseException();
    RNon IDeTesReference<RNon>.Roggi => throw new UnexpectedDeTesUseException();
    IOption<RNon> IDeTesReference<RNon>.RoggiUnstable => throw new UnexpectedDeTesUseException();
    IMemoryFZO IDeTesReference<RNon>.Memory => throw new UnexpectedDeTesUseException();
    int IDeTesSingleDomain.SelectedIndex() => throw new UnexpectedDeTesUseException();
}

internal class DeTesAssertIntegrityContext(int triggerIndex) : IDeTesContext
{
    public readonly int TriggerIndex = triggerIndex;
    private int? _accumulator;
    private IDeTesContext? _implementingContext;

    void IDeTesContext.AddAssertionMemory(Kor subject, Predicate<IMemoryFZO> assertion, string? description) =>
        Trigger(c => c.AddAssertionMemory(subject, _ => false, description));

    void IDeTesContext.AddAssertionRoggi<R>(IKorssa<R> subject, Predicate<R> assertion, string? description) =>
        Trigger(c => c.AddAssertionRoggi(subject, _ => false, description));

    void IDeTesContext.AddAssertionRoggiUnstable<R>(IKorssa<R> subject, Predicate<IOption<R>> assertion,
        string? description) => Trigger(c => c.AddAssertionRoggiUnstable(subject, _ => false, description));

    void IDeTesContext.AddAssertionKorssa<R>(IKorssa<R> subject, Predicate<IKorssa<R>> assertion, string? description) =>
        Trigger(c => c.AddAssertionKorssa(subject, _ => false, description));

    void IDeTesContext.MakeMultiSelectionDomain(Kor subject, int[][] selections,
        out IDeTesMultiDomain domainHandle, string? description) =>
        _implementingContext!.MakeMultiSelectionDomain(subject, selections, out domainHandle, description);

    void IDeTesContext.MakeReference<R>(IKorssa<R> subject, out IDeTesReference<R> reference, string? description) =>
        _implementingContext!.MakeReference(subject, out reference, description);

    void IDeTesContext.MakeSingleSelectionDomain(Kor subject, int[] selections,
        out IDeTesSingleDomain domainHandle, string? description) =>
        _implementingContext!.MakeSingleSelectionDomain(subject, selections, out domainHandle, description);

    // very weird flow.
    public IDeTesContext WithImplementingContext(IDeTesContext implementation)
    {
        _accumulator = 0;
        _implementingContext = implementation;
        return this;
    }

    private void Trigger(Action<IDeTesContext> action)
    {
        if (_accumulator is null) return;
        if (_accumulator != TriggerIndex)
        {
            _accumulator++;
            return;
        }

        action(_implementingContext!);
        //_implementingContext = null;
        _accumulator = null;
    }
}

internal interface IKnownException
{ }