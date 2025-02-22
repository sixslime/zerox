#nullable enable
namespace SixShaded.FourZeroOne.Rule.Defined.Matchers
{
    public record TypeMatcher<TMatch> : IRuleMatcher<TMatch>
        where TMatch : Tok
    {
        public bool MatchesToken(Tok token) => token is TMatch;
    }
}
