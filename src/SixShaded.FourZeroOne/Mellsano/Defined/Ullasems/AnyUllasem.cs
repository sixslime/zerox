namespace SixShaded.FourZeroOne.Mellsano.Defined.Ullasems;

public record AnyUllasem<TRestriction> : IUllasem<TRestriction>
    where TRestriction : Kor
{
    public required IPSet<IUllasem<TRestriction>> Entries { get; init; }
    public bool MatchesKorssa(Kor korssa) => Entries.Elements.Any(x => x.MatchesKorssa(korssa));
}