#nullable enable
using SixShaded.FourZeroOne;

namespace FourZeroOne.Rule
{
    using Core.Resolutions.Boxed;
    using FourZeroOne.FZOSpec;
    using Resolution.Unsafe;
    using Token;
    using Unsafe;
    using Define;
    using Proxies;
    using Token = IToken<Resolution.IResolution>;
    using Res = Resolution.IResolution;
    using System.Diagnostics.CodeAnalysis;
    using SixShaded.NotRust;
    using SixShaded.SixLib.GFunc;

    namespace Define
    {
        public record RuleForValue<RVal> : RuleBehavior<RVal>, IRuleOfValue<RVal>
        where RVal : class, Res
        {
            public required MetaFunction<OriginalProxy<RVal>, RVal> Definition { get; init; }
            public required IRuleMatcher<IHasNoArgs<RVal>> Matcher { get; init; }
            protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<RVal> token) => [];
            protected override IBoxedMetaFunction<RVal> InternalDefinition => Definition;
            protected override IRuleMatcher<IToken<RVal>> InternalMatcher => Matcher;
        }
        public record RuleForFunction<RArg1, ROut> : RuleBehavior<ROut>, IRuleOfFunction<RArg1, ROut>
            where RArg1 : class, Res
            where ROut : class, Res
        {
            public required MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut> Definition { get; init; }
            public required IRuleMatcher<IHasArgs<RArg1, ROut>> Matcher { get; init; }
            protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<ROut> token)
                => [CreateArgProxy<RArg1>(token.ArgTokens[0])];
            protected override IBoxedMetaFunction<ROut> InternalDefinition => Definition;
            protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
        }
        public record RuleForFunction<RArg1, RArg2, ROut> : RuleBehavior<ROut>, IRuleOfFunction<RArg1, RArg2, ROut>
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        {
            public required MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut> Definition { get; init; }
            public required IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>> Matcher { get; init; }
            protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<ROut> token)
                => [CreateArgProxy<RArg1>(token.ArgTokens[0]), CreateArgProxy<RArg2>(token.ArgTokens[1])];
            protected override IBoxedMetaFunction<ROut> InternalDefinition => Definition;
            protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
            
        }
        public record RuleForFunction<RArg1, RArg2, RArg3, ROut> : RuleBehavior<ROut>, IRuleOfFunction<RArg1, RArg2, RArg3, ROut>
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        {
            public required OverflowingMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut> Definition { get; init; }
            public required IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>> Matcher { get; init; }
            protected override IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<ROut> token)
                => [CreateArgProxy<RArg1>(token.ArgTokens[0]), CreateArgProxy<RArg2>(token.ArgTokens[1]), CreateArgProxy<RArg3>(token.ArgTokens[2])];
            protected override IBoxedMetaFunction<ROut> InternalDefinition => Definition;
            protected override IRuleMatcher<IToken<ROut>> InternalMatcher => Matcher;
        }
    }
    namespace Matchers
    {
        using Macro;
        using SixShaded.FourZeroOne;

        public record AnyMatcher<TRestriction> : IRuleMatcher<TRestriction>
        where TRestriction : Token
        {
            public required IPSet<IRuleMatcher<TRestriction>> Entries { get; init; }
            public bool MatchesToken(Token token) => Entries.Elements.Any(x => x.MatchesToken(token));
        }
        public record AllMatcher<TRestriction> : IRuleMatcher<TRestriction>
            where TRestriction : Token
        {
            public required IPSet<IRuleMatcher<TRestriction>> Entries { get; init; }
            public bool MatchesToken(Token token) => Entries.Elements.All(x => x.MatchesToken(token));
        }
        public record TypeMatcher<TMatch> : IRuleMatcher<TMatch>
            where TMatch : Token
        {
            public bool MatchesToken(Token token) => token is TMatch;
        }
        //DEV:
        // Macro matchers should be 'IRuleMatcher<Macro<...>>' but like thats not an interface and I dont want to make 4 IMacro<...>s
        public record MacroMatcher<RVal> : IRuleMatcher<IMacroValue<RVal>>
            where RVal : class, Res
        {
            public required MacroLabel Label { get; init; }
            public bool MatchesToken(Token token) => token is Macro<RVal> macro && macro.Label.Equals(Label);
        }
        public record MacroMatcher<RArg1, ROut> : IRuleMatcher<IMacroFunction<RArg1, ROut>>
            where RArg1 : class, Res
            where ROut : class, Res
        {
            public required MacroLabel Label { get; init; }
            public bool MatchesToken(Token token) => token is Macro<RArg1, ROut> macro && macro.Label.Equals(Label);
        }
        public record MacroMatcher<RArg1, RArg2, ROut> : IRuleMatcher<IMacroFunction<RArg1, RArg2, ROut>>
            where RArg1 : class, Res
            where RArg2 : class, Res
            where ROut : class, Res
        {
            public required MacroLabel Label { get; init; }
            public bool MatchesToken(Token token) => token is Macro<RArg1, RArg2, ROut> macro && macro.Label.Equals(Label);
        }
        public record MacroMatcher<RArg1, RArg2, RArg3, ROut> : IRuleMatcher<IMacroFunction<RArg1, RArg2, RArg3, ROut>>
            where RArg1 : class, Res
            where RArg2 : class, Res
            where RArg3 : class, Res
            where ROut : class, Res
        {
            public required MacroLabel Label { get; init; }
            public bool MatchesToken(Token token) => token is Macro<RArg1, RArg2, RArg3, ROut> macro && macro.Label.Equals(Label);
        }
    }
    namespace Proxies
    {
        public record RealizeProxy<R> : RuntimeHandledFunction<IProxy<R>, R>
            where R : class, Res
        {
            public RealizeProxy(IToken<IProxy<R>> proxy) : base(proxy) { }
            protected override EStateImplemented MakeData(IProxy<R> proxy)
            {
                return new EStateImplemented.MetaExecute
                {
                    Token = proxy.Token,
                    RuleAllows = proxy.ReallowsRule ? proxy.FromRule.Yield() : [],
                };
            }
        }
        public abstract record ProxyBehavior<R> : Resolution.NoOp, IProxy<R>
            where R : class, Res
        {
            public required IToken<R> Token { get; init; }
            public required RuleID FromRule { get; init; }
            public abstract bool ReallowsRule { get; }
        }
        public record OriginalProxy<R> : ProxyBehavior<R> where R : class, Res
        {
            public override bool ReallowsRule => false;
        }
        
        public record ArgProxy<R> : ProxyBehavior<R> where R : class, Res
        { public override bool ReallowsRule => true; }
        
    }

    // WARNING:
    // unsafe assumptions, puts full responsibility on Rules to hold safety.
    public abstract record RuleBehavior<R> : IRule<R>
        where R : class, Res
    {
        public RuleID ID { get; } = RuleIDGenerator.Next();
        protected abstract IRuleMatcher<IToken<R>> InternalMatcher { get; }
        protected abstract IBoxedMetaFunction<R> InternalDefinition { get; }

        /// <summary>
        /// Kinda stupid that this has to exist, but the alternative is dynamic instantiation via reflection in TryApply(). <br></br>
        /// <i>Or I guess fully type-unsafe proxies but who wants that.</i>
        /// </summary>
        protected abstract IEnumerable<IProxy<Res>> ConstructArgProxies(IToken<R> token);
        /// <summary>
        /// <paramref name="token"/> is casted to token of <typeparamref name="RArg"/>.
        /// </summary>
        /// <typeparam name="RArg"></typeparam>
        /// <param name="token"></param>
        /// <returns></returns>
        protected ArgProxy<RArg> CreateArgProxy<RArg>(Token token)
            where RArg : class, Res
        {
            return new() { FromRule = ID, Token = token.IsA<IToken<RArg>>() };
        }
        IRuleMatcher<IToken<R>> IRule<R>.MatcherUnsafe => InternalMatcher;
        IBoxedMetaFunction<R> IRule<R>.DefinitionUnsafe => InternalDefinition;

        public IOption<IRuledToken<R>> TryApply(Token token)
        {
            return (token is IToken<R> typed && InternalMatcher.MatchesToken(token))
                ? new RuledToken<R>
                {
                    AppliedRule = this,
                    Proxies =
                        new OriginalProxy<R>() { Token = typed, FromRule = ID }.IsA<IProxy<Res>>().Yield()
                        .Concat(ConstructArgProxies(typed))
                        .ToArray()
                }.AsSome()
                : new None<IRuledToken<R>>();
        }
    }

    public record RuledToken<R> : RuntimeHandledValue<R>, IRuledToken<R>
        where R : class, Res
    {
        public required IRule<R> AppliedRule { get; init; }

        // [0] is always self/original proxy, rest are arg proxies in-order.
        public required IProxy<Res>[] Proxies { get; init; }
        protected override EStateImplemented MakeData()
        {
            var definition = AppliedRule.DefinitionUnsafe;
            return new EStateImplemented.MetaExecute()
            {
                Token = definition.Token,
                ObjectWrites =
                    definition.SelfIdentifier.IsA<Resolution.IMemoryAddress<Res>>().Yield()
                    .Concat(definition.ArgAddresses)
                    .ZipShort(
                        definition.IsA<Res>().Yield()
                        .Concat(Proxies)
                        .Map(x => x.AsSome()))
                    .Tipled(),
                RuleMutes = [AppliedRule.ID]
            };
        }
    }

    public interface IProxy<out R> : Res
            where R : class, Res
    {
        public IToken<R> Token { get; }
        public RuleID FromRule { get; }
        public bool ReallowsRule { get; }
    }

    public interface IRuleOfValue<RVal> : IRule<RVal>
    where RVal : class, Res
    {
        public MetaFunction<OriginalProxy<RVal>, RVal> Definition { get; }
        public IRuleMatcher<IHasNoArgs<RVal>> Matcher { get; }
    }
    public interface IRuleOfFunction<RArg1, ROut> : IRule<ROut>
        where RArg1 : class, Res
        where ROut : class, Res
    {
        public MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ROut> Definition { get; }
        public IRuleMatcher<IHasArgs<RArg1, ROut>> Matcher { get; }
    }
    public interface IRuleOfFunction<RArg1, RArg2, ROut> : IRule<ROut>
        where RArg1 : class, Res
        where RArg2 : class, Res
        where ROut : class, Res
    {
        public MetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ROut> Definition { get; }
        public IRuleMatcher<IHasArgs<RArg1, RArg2, ROut>> Matcher { get; }
    }
    public interface IRuleOfFunction<RArg1, RArg2, RArg3, ROut> : IRule<ROut>
        where RArg1 : class, Res
        where RArg2 : class, Res
        where RArg3 : class, Res
        where ROut : class, Res
    {
        public OverflowingMetaFunction<OriginalProxy<ROut>, ArgProxy<RArg1>, ArgProxy<RArg2>, ArgProxy<RArg3>, ROut> Definition { get; }
        public IRuleMatcher<IHasArgs<RArg1, RArg2, RArg3, ROut>> Matcher { get; }
    }

    public interface IRuleMatcher<out TRestriction>
        where TRestriction : Token
    {
        public bool MatchesToken(Token token);
    }

    
    public static class RuleIDGenerator
    {
        private static int _currentID = 0;
        public static RuleID Next() => new(_currentID++);
    }

    public readonly struct RuleID(int id)
    {
        public readonly int ID = id;
        public override string ToString() => $"RuleID({ID})";
    }
    
    namespace Unsafe
    {
        public interface IRule<out R>
        where R : class, Res
        {
            public RuleID ID { get; }
            public IRuleMatcher<IToken<R>> MatcherUnsafe { get; }
            public IBoxedMetaFunction<R> DefinitionUnsafe { get; }
            public IOption<IRuledToken<R>> TryApply(Token token);
        }
        public interface IRuledToken<out R> : IToken<R>
        where R : class, Res
        {
            public IRule<R> AppliedRule { get; }
        }
    }

    
}