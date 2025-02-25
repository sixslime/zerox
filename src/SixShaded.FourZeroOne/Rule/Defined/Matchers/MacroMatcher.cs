namespace SixShaded.FourZeroOne.Rule.Defined.Matchers;

public record MacroMatcher<RVal> : IRuleMatcher<IMacroSignature<RVal>>
    where RVal : class, Res
{
    public required MacroLabel Label { get; init; }
    public bool MatchesToken(Tok token) => token is Macro<RVal> macro && macro.Label.Equals(Label);
}
public record MacroMatcher<RArg1, ROut> : IRuleMatcher<IMacroSignature<RArg1, ROut>>
    where RArg1 : class, Res
    where ROut : class, Res
{
    public required MacroLabel Label { get; init; }
    public bool MatchesToken(Tok token) => token is Macro<RArg1, ROut> macro && macro.Label.Equals(Label);
}
public record MacroMatcher<RArg1, RArg2, ROut> : IRuleMatcher<IMacroSignature<RArg1, RArg2, ROut>>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where ROut : class, Res
{
    public required MacroLabel Label { get; init; }
    public bool MatchesToken(Tok token) => token is Macro<RArg1, RArg2, ROut> macro && macro.Label.Equals(Label);
}
public record MacroMatcher<RArg1, RArg2, RArg3, ROut> : IRuleMatcher<IMacroSignature<RArg1, RArg2, RArg3, ROut>>
    where RArg1 : class, Res
    where RArg2 : class, Res
    where RArg3 : class, Res
    where ROut : class, Res
{
    public required MacroLabel Label { get; init; }
    public bool MatchesToken(Tok token) => token is Macro<RArg1, RArg2, RArg3, ROut> macro && macro.Label.Equals(Label);
}