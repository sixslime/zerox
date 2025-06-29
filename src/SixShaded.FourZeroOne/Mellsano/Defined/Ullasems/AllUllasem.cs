namespace SixShaded.FourZeroOne.Mellsano.Defined.Ullasems;

public record AllUllasem<TRestriction> : IUllasem<TRestriction>
    where TRestriction : Kor
{
    public required IPSet<IUllasem<TRestriction>> Entries { get; init; }
    public bool MatchesKorssa(Kor korssa) => Entries.Elements.All(x => x.MatchesKorssa(korssa));
    public override string ToString() => string.Join(" & ", Entries.Elements);
}