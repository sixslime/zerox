namespace SixShaded.MinimaFZO;

using FourZeroOne.Mellsano;
using FourZeroOne.Roggi;
using FourZeroOne.Roveggi;

public record MinimaMemoryFZO : IMemoryFZO
{
    private PMap<MellsanoID, int> _mellsanoMutes;
    private PSequence<Mel> _mellsanos;
    private PMap<Addr, Rog> _objects;

    public MinimaMemoryFZO()
    {
        _objects = new();
        _mellsanos = new();
        _mellsanoMutes = new();
    }

    IEnumerable<ITiple<Addr, Rog>> IMemoryFZO.Objects => _objects.Elements;
    IEnumerable<Mel> IMemoryFZO.Mellsanos => _mellsanos.Elements;
    IEnumerable<ITiple<MellsanoID, int>> IMemoryFZO.MellsanoMutes => _mellsanoMutes.Elements;
    IOption<R> IMemoryFZO.GetObject<R>(IRoda<R> address) => _objects.At(address).RemapAs(x => (R)x);

    public IEnumerable<ITiple<IRoveggi<D>, R>> GetRovedanggiAssignmentsOfType<D, R>()
        where D : Rovedantu<R>
        where R : class, Rog =>
        _objects.Elements.FilterMap(
        x =>
            x.A is RovedanggiWrapper<R>
            {
                Roveggi: IRoveggi<D> roveggi
            }
                ? (roveggi, (R)x.B).Tiple().AsSome()
                : new None<ITiple<IRoveggi<D>, R>>());

    int IMemoryFZO.GetMellsanoMuteCount(MellsanoID mellsanoId) => _mellsanoMutes.At(mellsanoId).Or(0);

    IMemoryFZO IMemoryFZO.WithMellsanos(IEnumerable<Mel> mellsanos) =>
        this with
        {
            _mellsanos = _mellsanos.WithEntries(mellsanos),
        };

    IMemoryFZO IMemoryFZO.WithObjects<R>(IEnumerable<ITiple<IRoda<R>, R>> insertions) =>
        this with
        {
            _objects = _objects.WithEntries(insertions),
        };

    IMemoryFZO IMemoryFZO.WithClearedAddresses(IEnumerable<Addr> removals) =>
        this with
        {
            _objects = _objects.WithoutEntries(removals),
        };

    IMemoryFZO IMemoryFZO.WithMellsanoMutes(IEnumerable<MellsanoID> mutes) =>
        this with
        {
            _mellsanoMutes =
            _mellsanoMutes.WithEntries(
            mutes.Map(
                mute =>
                    (mute, _mellsanoMutes.At(mute).Or(0) + 1))
                .Tipled()),
        };

    IMemoryFZO IMemoryFZO.WithoutMellsanoMutes(IEnumerable<MellsanoID> mutes) =>
        this with
        {
            _mellsanoMutes =
            mutes.AccumulateInto(
            _mellsanoMutes,
            (set, x) =>
                set.WithEntries((x, set.At(x).Or(0) - 1).Tiple())
                    .WithoutEntries<PMap<MellsanoID, int>, MellsanoID>(set.At(x).Or(0) > 0 ? [] : [x])),
        };
}