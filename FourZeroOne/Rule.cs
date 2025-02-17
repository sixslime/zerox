using Perfection;
using ResObj = FourZeroOne.Resolution.IResolution;
#nullable enable
namespace FourZeroOne.Rule
{
    using Core.Resolutions.Boxed;
    using FourZeroOne.FZOSpec;
    using Resolution.Unsafe;
    using Token;
    using Unsafe;
    using Define;
    using Proxies;
    using any_token = Token.IToken<ResObj>;

    
    namespace Define
    {
        public record RuleForValue<RVal> : RuleBehavior<RVal>, IRuleOfValue<RVal>
        where RVal : class, ResObj
        {
            public required MetaFunction<OriginalProxy<RVal>, RVal> Definition { get; init; }
            public required IRuleMatcher<IHasNoArgs<RVal>> Matcher { get; init; }
            protected override IBoxedMetaFunction<RVal> InternalDefinition => Definition;
            protected override IRuleMatcher<IToken<RVal>> InternalMatcher => Matcher;
        }
        public record RuleForFunction<RArg1, ROut> : RuleBehavior<ROut>, IRuleOfFunction<RArg1, ROut>
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            public required MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut> Definition { get; init; }
            public required IRuleMatcher<IHasArgs<RArg1, ROut>> Matcher { get; init; }
            protected override IBoxedMetaFunction<ROut> InternalDefinition => Definition;
            protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
        }
        public record RuleForFunction<RArg1, RArg2, ROut> : RuleBehavior<ROut>, IRuleOfFunction<RArg1, RArg2, ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            public required MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut> Definition { get; init; }
            public required IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>> Matcher { get; init; }
            protected override IBoxedMetaFunction<ROut> InternalDefinition => Definition;
            protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
        }
        public record RuleForFunction<TMatch, RArg1, RArg2, RArg3, ROut> : RuleBehavior<ROut>, IRuleOfFunction<RArg1, RArg2, RArg3, ROut>
            where TMatch : IHasArgs<RArg1, RArg2, RArg3, ROut>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            public required OverflowingMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut> Definition { get; init; }
            public required IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>> Matcher { get; init; }
            protected override IBoxedMetaFunction<ROut> InternalDefinition => Definition;
            protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
        }
    }
    namespace Matchers
    {
        public record AnyMatcher<TRestriction> : IRuleMatcher<TRestriction>
        where TRestriction : any_token
        {
            public required IHasElements<IRuleMatcher<TRestriction>> Entries { get; init; }
            public bool MatchesToken(any_token token) => Entries.Elements.Any(x => x.MatchesToken(token));
        }
        public record AllMatcher<TRestriction> : IRuleMatcher<TRestriction>
            where TRestriction : any_token
        {
            public required IHasElements<IRuleMatcher<TRestriction>> Entries { get; init; }
            public bool MatchesToken(any_token token) => Entries.Elements.All(x => x.MatchesToken(token));
        }
        public record TypeMatcher<TMatch> : IRuleMatcher<TMatch>
            where TMatch : any_token
        {
            public bool MatchesToken(any_token token) => token is TMatch;
        }
        public record MacroMatcher<RVal> : IRuleMatcher<Macro.Macro<RVal>>
            where RVal : class, ResObj
        {
            public required Macro.MacroLabel Label { get; init; }
            public bool MatchesToken(any_token token) => token is Macro.Macro<RVal> macro && macro.Label.Equals(Label);
        }
        public record MacroMatcher<RArg1, ROut> : IRuleMatcher<Macro.Macro<RArg1, ROut>>
            where RArg1 : class, ResObj
            where ROut : class, ResObj
        {
            public required Macro.MacroLabel Label { get; init; }
            public bool MatchesToken(any_token token) => token is Macro.Macro<RArg1, ROut> macro && macro.Label.Equals(Label);
        }
        public record MacroMatcher<RArg1, RArg2, ROut> : IRuleMatcher<Macro.Macro<RArg1, RArg2, ROut>>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where ROut : class, ResObj
        {
            public required Macro.MacroLabel Label { get; init; }
            public bool MatchesToken(any_token token) => token is Macro.Macro<RArg1, RArg2, ROut> macro && macro.Label.Equals(Label);
        }
        public record MacroMatcher<RArg1, RArg2, RArg3, ROut> : IRuleMatcher<Macro.Macro<RArg1, RArg2, RArg3, ROut>>
            where RArg1 : class, ResObj
            where RArg2 : class, ResObj
            where RArg3 : class, ResObj
            where ROut : class, ResObj
        {
            public required Macro.MacroLabel Label { get; init; }
            public bool MatchesToken(any_token token) => token is Macro.Macro<RArg1, RArg2, RArg3, ROut> macro && macro.Label.Equals(Label);
        }
    }
    namespace Proxies
    {
        public record RealizeProxy<R> : RuntimeHandledFunction<IProxy<R>, R>
    where R : class, ResObj
        {
            public RealizeProxy(IToken<IProxy<R>> proxy) : base(proxy) { }
            protected override EStateImplemented MakeData(IProxy<R> proxy)
            {
                return new EStateImplemented.MetaExecute
                {
                    Token = proxy.Token,
                    RuleAllows = new PSequence<RuleID>().WithEntries(proxy.ReallowsRule ? proxy.FromRule.Yield() : []),
                };
            }
        }
        public abstract record ProxyBehavior<R> : Resolution.NoOp, IProxy<R>
            where R : class, ResObj
        {
            public required IToken<R> Token { get; init; }
            public required RuleID FromRule { get; init; }
            public abstract bool ReallowsRule { get; }
        }
        public record OriginalProxy<R> : ProxyBehavior<R> where R : class, ResObj
        { public override bool ReallowsRule => false; }
        public record ArgProxy<R> : ProxyBehavior<R> where R : class, ResObj
        { public override bool ReallowsRule => true; }
        
    }

    // WARNING:
    // unsafe assumptions, puts full responsibility on Rules to hold safety.
    public abstract record RuleBehavior<R> : IRule<R>
        where R : class, ResObj
    {
        public RuleID ID => RuleIDGenerator.Next();
        protected abstract IRuleMatcher<IToken<R>> InternalMatcher { get; }
        IRuleMatcher<IToken<R>> IRule<R>.MatcherUnsafe => InternalMatcher;
        protected abstract IBoxedMetaFunction<R> InternalDefinition { get; }
        IBoxedMetaFunction<R> IRule<R>.DefinitionUnsafe => InternalDefinition;

        public IOption<IRuledToken<R>> TryApply(any_token token)
        {
            return (token is IToken<R> && InternalMatcher.MatchesToken(token))
                .ToOptionLazy(() => new RuledToken<R>()
                {
                    AppliedRule = this,
                    // this on its own is very unsafe, it's safety is assumed to be enforced by Rules only allowing correct definitions.
                    Proxies =
                        token.Yield()
                        .Concat(token.ArgTokens)
                        .Enumerate()
                        .Map(x =>
                        (x.index > 0)
                            ? new ArgProxy<ResObj>() { Token = x.value, FromRule = ID }.IsA<IProxy<ResObj>>()
                            : new OriginalProxy<ResObj>() { Token = x.value, FromRule = ID }.IsA<IProxy<ResObj>>())
                        .ToArray()
                });
        }
    }

    public record RuledToken<R> : RuntimeHandledValue<R>, IRuledToken<R>
        where R : class, ResObj
    {
        public required IRule<R> AppliedRule { get; init; }

        // [0] is always self/original proxy, rest are arg proxies in-order.
        public required IProxy<ResObj>[] Proxies { get; init; }
        protected override EStateImplemented MakeData()
        {
            var definition = AppliedRule.DefinitionUnsafe;
            return new EStateImplemented.MetaExecute()
            {
                Token = definition.Token,
                ObjectWrites =
                    definition.SelfIdentifier.IsA<Resolution.IMemoryAddress<ResObj>>().Yield()
                    .Concat(definition.ArgAddresses)
                    .ZipShort(
                        definition.IsA<ResObj>().Yield()
                        .Concat(Proxies)
                        .Map(x => x.AsSome()))
                    .Tipled()
                    .ToPSequence(),
                RuleMutes = AppliedRule.ID.Yield().ToPSequence()
            };
        }
    }

    public interface IProxy<out R> : ResObj
            where R : class, ResObj
    {
        public IToken<R> Token { get; }
        public RuleID FromRule { get; }
        public bool ReallowsRule { get; }
    }

    public interface IRuleOfValue<RVal> : IRule<RVal>
    where RVal : class, ResObj
    {
        public MetaFunction<OriginalProxy<RVal>, RVal> Definition { get; }
        public IRuleMatcher<IHasNoArgs<RVal>> Matcher { get; }
    }
    public interface IRuleOfFunction<RArg1, ROut> : IRule<ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut> Definition { get; }
        public IRuleMatcher<IHasArgs<RArg1, ROut>> Matcher { get; }
    }
    public interface IRuleOfFunction<RArg1, RArg2, ROut> : IRule<ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where ROut : class, ResObj
    {
        public MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut> Definition { get; }
        public IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>> Matcher { get; }
    }
    public interface IRuleOfFunction<RArg1, RArg2, RArg3, ROut> : IRule<ROut>
        where RArg1 : class, ResObj
        where RArg2 : class, ResObj
        where RArg3 : class, ResObj
        where ROut : class, ResObj
    {
        public OverflowingMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut> Definition { get; }
        public IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>> Matcher { get; }
    }

    public interface IRuleMatcher<out TRestriction>
        where TRestriction : any_token
    {
        public bool MatchesToken(any_token token);
    }

    
    public static class RuleIDGenerator
    {
        private static int _currentID = 0;
        public static RuleID Next() => new(_currentID++);
    }

    public readonly struct RuleID(int id) { public readonly int ID = id; }
    
    namespace Unsafe
    {
        public interface IRule<out R>
        where R : class, ResObj
        {
            public RuleID ID { get; }
            public IRuleMatcher<IToken<R>> MatcherUnsafe { get; }
            public IBoxedMetaFunction<R> DefinitionUnsafe { get; }
            public IOption<IRuledToken<R>> TryApply(any_token token);
        }
        public interface IRuledToken<out R> : IToken<R>
        where R : class, ResObj
        {
            public IRule<R> AppliedRule { get; }
        }
    }

    
}