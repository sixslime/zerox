namespace SixShaded.FourZeroOne.FZOSpec;

public interface IMemoryFZO
{
    public IEnumerable<ITiple<IRoda<>, Rog>> Objects { get; }
    public IEnumerable<Mel> Mellsanos { get; }
    public IEnumerable<ITiple<MellsanoID, int>> MellsanoMutes { get; }

    public IOption<R> GetObject<R>(IRoda<R> address)
        where R : class, Rog;

    public int GetMellsanoMuteCount(MellsanoID mellsanoId);

    // Mellsanos is an ordered sequence allowing duplicates.
    // 'WithMellsanos' appends.
    // 'WithoutMellsanos' removes the *first* instance of each mellsano in sequence.
    // Mellsanos have a public ID assigned at creation, equality is based on ID.
    public IMemoryFZO WithMellsanos(IEnumerable<Mel> mellsanos);
    public IMemoryFZO WithMellsanoMutes(IEnumerable<MellsanoID> mutes);
    public IMemoryFZO WithoutMellsanoMutes(IEnumerable<MellsanoID> mutes);

    public IMemoryFZO WithObjects<R>(IEnumerable<ITiple<IRoda<R>, R>> insertions)
        where R : class, Rog;

    public IMemoryFZO WithClearedAddresses(IEnumerable<IRoda<>> removals);
}