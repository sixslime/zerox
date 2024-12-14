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
        public IProxy<TOrig, R> WithHookLabels(IEnumerable<string> labels);
    }
    public abstract record Proxy<TOrig, R> : IProxy<TOrig, R> where TOrig : IToken where R : class, ResObj
    {
        // HookLabels are appended to ArgTransform.
        // This is a cringe :P
        public IEnumerable<string> HookLabels => _hookLabels.Elements;
        public Proxy()
        {
            _hookLabels = new() { Elements = [] };
        }
        protected abstract IToken<R> RealizeInternal(TOrig original, IOption<Rule.IRule> realizingRule);
        public IToken<R> Realize(TOrig original, IOption<Rule.IRule> realizingRule)
        {
            return RealizeInternal(original, realizingRule)
                .ExprAs(x => x.WithHookLabels(x.HookLabels.Also(HookLabels)));
        }
        public IToken<R> UnsafeTypedRealize(IToken original, IOption<Rule.IRule> rule) => Realize((TOrig)original, rule);
        public IToken UnsafeContextualRealize(TOrig original, IOption<Rule.IRule> rule) => Realize(original, rule);
        public IToken UnsafeRealize(IToken original, IOption<Rule.IRule> rule) => UnsafeTypedRealize(original, rule);
        public Unsafe.IProxy UnsafeWithHookLabels(IEnumerable<string> labels) => WithHookLabels(labels);
        public IProxy<TOrig, R> WithHookLabels(IEnumerable<string> labels) => this with { _hookLabels = new() { Elements = labels } };
        protected static IToken<RLocal> RuleApplied<RLocal>(IOption<Rule.IRule> rule, IToken<RLocal> original) where RLocal : class, ResObj
        {
            return rule.RemapAs(r => r.TryApplyTyped(original)).Press().Or(original);
        }
        protected static IToken RuleAppliedUnsafe(IOption<Rule.IRule> rule, IToken original)
        {
            return rule.RemapAs(r => r.TryApply(original)).Press().Or(original);
        }
        private PSet<string> _hookLabels;
    }
}
namespace FourZeroOne.Proxy.Unsafe
{
    using ResObj = Resolution.IResolution;
    using Token.Unsafe;
    using Token;

    public interface IProxy
    {
        public IEnumerable<string> HookLabels { get; }
        public IToken UnsafeRealize(IToken original, IOption<Rule.IRule> rule);
        public IProxy UnsafeWithHookLabels(IEnumerable<string> labels);
    }
    public interface IProxy<out R> : IProxy where R : class, ResObj
    {
        public abstract IToken<R> UnsafeTypedRealize(IToken original, IOption<Rule.IRule> rule);
    }
    public interface IProxyOf<in TOrig> : IProxy where TOrig : IToken
    {
        public abstract IToken UnsafeContextualRealize(TOrig original, IOption<Rule.IRule> rule);
    }

    // DEV: perhaps make FunctionProxy follow the same structure as TransformProxy
    public abstract record ThisProxy<TOrig, R> : Proxy<TOrig, R> where TOrig : IToken<R> where R : class, ResObj
    {
        public IEnumerable<string> HookLabelRemovals { get => RemovedHookLabels.Elements; init => RemovedHookLabels = new() { Elements = value }; }
        protected readonly PList<IProxy> ArgProxies;
        protected readonly PSet<string> RemovedHookLabels;
        protected ThisProxy(IEnumerable<string> hookRemovals, IEnumerable<IProxy> proxies)
        {
            ArgProxies = new() { Elements = proxies };
            RemovedHookLabels = new() { Elements = hookRemovals };
        }
        // NOTE: 'hookRemovals' may be unnecessary
        protected ThisProxy(IEnumerable<string> hookRemovals, params IProxy[] proxies) : this(hookRemovals, proxies.IEnumerable()) { }
        protected override IToken<R> RealizeInternal(TOrig original, IOption<Rule.IRule> rule)
        {
            return original.UnsafeTypedWithArgs(
                ArgProxies.Elements.ZipLong(original.ArgTokens)
                .Map(optPair => optPair.a.RemapAs(argProxy => argProxy.UnsafeRealize(original, rule))
                        .Or(RuleAppliedUnsafe(rule, optPair.b.Unwrap())))
                .ToArray())
            .WithHookLabels(original.HookLabels.Except(RemovedHookLabels.Elements));
        }
    }
    public abstract record FunctionProxy<TOrig, R> : Proxy<TOrig, R>
        where TOrig : IToken
        where R : class, ResObj
    {
        protected sealed override IToken<R> RealizeInternal(TOrig original, IOption<Rule.IRule> rule)
        {
            var o = ConstructFromArgs(original, MakeSubstitutions(original, rule));
            // this is a little silly.
            return o.WithHookLabels([.. o.HookLabels.Also(HookLabels)]);
        }

        protected readonly PList<IProxy> ArgProxies;
        protected abstract IToken<R> ConstructFromArgs(TOrig original, List<IToken> tokens);
        protected FunctionProxy(IEnumerable<IProxy> proxies)
        {
            ArgProxies = new() { Elements = proxies };
        }
        protected FunctionProxy(params IProxy[] proxies) : this(proxies.IEnumerable()) { }
        protected List<IToken> MakeSubstitutions(TOrig original, IOption<Rule.IRule> rule)
        {
            return new(ArgProxies.Elements.Map(x => x.UnsafeRealize(original, rule)));
        }
    }
}