namespace SixShaded.FourZeroOne.Mellsano;

public interface IUllasem<out TRestriction>
    where TRestriction : Kor
{
    public bool MatchesKorssa(Kor korssa);
}