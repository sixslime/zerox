#nullable enable

namespace SixShaded.FourZeroOne.Rule.Defined.Matchers
{
    public record AllMatcher<TRestriction> : IRuleMatcher<TRestriction>
        where TRestriction : Token
    {
        public required IPSet<IRuleMatcher<TRestriction>> Entries { get; init; }
        public bool MatchesToken(Token token) => Entries.Elements.All(x => x.MatchesToken(token));
    }
}
