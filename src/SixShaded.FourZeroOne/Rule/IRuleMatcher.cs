namespace SixShaded.FourZeroOne.Rule;

public interface IRuleMatcher<out TRestriction>
    where TRestriction : Tok
{
    public bool MatchesToken(Tok token);
}