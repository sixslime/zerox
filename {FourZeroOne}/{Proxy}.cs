using System.Collections;
using System.Collections.Generic;
using Perfection;
using System;

#nullable enable
namespace FourZeroOne.Proxy
{
    using ResObj = Resolution.IResolution;
    using Token.Unsafe;
    using Token;
    public interface IProxy<in TOrig, out R> : Unsafe.IProxyOf<TOrig>, Unsafe.IProxy<R> where TOrig : IToken where R : class, ResObj
    {
        public IToken<R> Realize(TOrig original, IOption<Rule.IRule> realizingRule);
    }
    public abstract record Proxy<TOrig, R> : IProxy<TOrig, R> where TOrig : IToken where R : class, ResObj
    {
        public abstract IToken<R> Realize(TOrig original, IOption<Rule.IRule> realizingRule);
        public IToken<R> UnsafeTypedRealize(IToken original, IOption<Rule.IRule> rule) { return Realize((TOrig)original, rule); }
        public IToken UnsafeContextualRealize(TOrig original, IOption<Rule.IRule> rule) { return Realize(original, rule); }
        public IToken UnsafeRealize(IToken original, IOption<Rule.IRule> rule) => UnsafeTypedRealize(original, rule);

        protected static IToken<RLocal> RuleApplied<RLocal>(IOption<Rule.IRule> rule, IToken<RLocal> original) where RLocal : class, ResObj
        {
            return rule.RemapAs(r => r.TryApplyTyped(original).Or(original)).Or(original);
        }
    }
}
namespace FourZeroOne.Proxy.Unsafe
{
    using ResObj = Resolution.IResolution;
    using Token.Unsafe;
    using Token;
    public interface IProxy
    {
        public IToken UnsafeRealize(IToken original, IOption<Rule.IRule> rule);
    }
    public interface IProxy<out R> : IProxy where R : class, ResObj
    {
        public abstract IToken<R> UnsafeTypedRealize(IToken original, IOption<Rule.IRule> rule);
    }
    public interface IProxyOf<in TOrig> : IProxy where TOrig : IToken
    {
        public abstract IToken UnsafeContextualRealize(TOrig original, IOption<Rule.IRule> rule);
    }

    public abstract record FunctionProxy<TOrig, R> : Proxy<TOrig, R>
        where TOrig : IToken
        where R : class, ResObj
    {
        public sealed override IToken<R> Realize(TOrig original, IOption<Rule.IRule> rule) { return ConstructFromArgs(original, MakeSubstitutions(original, rule)); }

        protected readonly PList<IProxy> ArgProxies;
        protected abstract IToken<R> ConstructFromArgs(TOrig original, List<IToken> tokens);
        protected FunctionProxy(IEnumerable<IProxy> proxies)
        {
            ArgProxies = new() { Elements = proxies };
        }
        protected FunctionProxy(params IProxy[] proxies) : this(proxies as IEnumerable<IProxy>) { }
        protected List<IToken> MakeSubstitutions(TOrig original, IOption<Rule.IRule> rule)
        {
            return new(ArgProxies.Elements.Map(x => x.UnsafeRealize(original, rule)));
        }
    }
}