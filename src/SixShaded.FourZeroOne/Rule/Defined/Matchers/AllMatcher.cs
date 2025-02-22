namespace SixShaded.FourZeroOne.Rule.Defined.Matchers;

public record AllMatcher<TRestriction> : IRuleMatcher<TRestriction>
    where TRestriction : Tok
{
    public required IPSet<IRuleMatcher<TRestriction>> Entries { get; init; }
    public bool MatchesToken(Tok token) => Entries.Elements.All(x => x.MatchesToken(token));
}