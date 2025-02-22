#nullable enable
namespace SixShaded.FourZeroOne.Rule.Defined.Matchers
{
    public record AnyMatcher<TRestriction> : IRuleMatcher<TRestriction>
        where TRestriction : Token
    {
        public required IPSet<IRuleMatcher<TRestriction>> Entries { get; init; }
        public bool MatchesToken(Token token) => Entries.Elements.Any(x => x.MatchesToken(token));
    }
}
