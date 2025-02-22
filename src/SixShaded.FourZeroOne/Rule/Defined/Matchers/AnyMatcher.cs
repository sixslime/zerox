namespace SixShaded.FourZeroOne.Rule.Defined.Matchers;

public record AnyMatcher<TRestriction> : IRuleMatcher<TRestriction>
    where TRestriction : Tok
{
    public required IPSet<IRuleMatcher<TRestriction>> Entries { get; init; }
    public bool MatchesToken(Tok token) => Entries.Elements.Any(x => x.MatchesToken(token));
}