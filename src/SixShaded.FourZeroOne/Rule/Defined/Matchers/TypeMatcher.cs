#nullable enable
namespace SixShaded.FourZeroOne.Rule.Defined.Matchers
{
    public record TypeMatcher<TMatch> : IRuleMatcher<TMatch>
        where TMatch : Token
    {
        public bool MatchesToken(Token token) => token is TMatch;
    }
}
