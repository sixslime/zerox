#nullable enable
namespace SixShaded.FourZeroOne.Rule
{
    public interface IRuleMatcher<out TRestriction>
        where TRestriction : Token
    {
        public bool MatchesToken(Token token);
    }
}
