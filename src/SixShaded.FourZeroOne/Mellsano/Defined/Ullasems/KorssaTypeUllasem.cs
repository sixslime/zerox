namespace SixShaded.FourZeroOne.Mellsano.Defined.Ullasems;

public record KorssaTypeUllasem<TMatch> : IUllasem<TMatch>
    where TMatch : Kor
{
    public bool MatchesKorssa(Kor korssa) => korssa is TMatch;
}