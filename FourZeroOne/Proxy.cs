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
        public IProxy<TOrig, R> WithLabels(IPSet<string> labels);
    }
    public abstract record Proxy<TOrig, R> : IProxy<TOrig, R> where TOrig : IToken where R : class, ResObj
    {
        // HookLabels are appended to ArgTransform.
        // This is a cringe :P
        public IPSet<string> Labels => _labels;
        public Proxy()
        {
            _labels = new();
        }
        protected abstract IToken<R> RealizeInternal(TOrig original, IOption<Rule.IRule> realizingRule);
        public IToken<R> Realize(TOrig original, IOption<Rule.IRule> realizingRule)
        {
            return RealizeInternal(original, realizingRule)
                .ExprAs(x => x.WithLabels(x.Labels.Also(Labels)));
        }
        public IToken<R> UnsafeTypedRealize(IToken original, IOption<Rule.IRule> rule) => Realize((TOrig)original, rule);
        public IToken UnsafeContextualRealize(TOrig original, IOption<Rule.IRule> rule) => Realize(original, rule);
        public IToken UnsafeRealize(IToken original, IOption<Rule.IRule> rule) => UnsafeTypedRealize(original, rule);
        public Unsafe.IProxy UnsafeWithLabels(IPSet<string> labels) => WithLabels(labels);
        public IProxy<TOrig, R> WithLabels(params string[] labels) => WithLabels(labels.ToPSet());
        public IProxy<TOrig, R> WithLabels(IPSet<string> labels) => this with { _labels = labels };
        protected static IToken<RLocal> RuleApplied<RLocal>(IOption<Rule.IRule> rule, IToken<RLocal> original) where RLocal : class, ResObj
        {
            return rule.RemapAs(r => r.TryApplyTyped(original)).Press().Or(original);
        }
        protected static IToken RuleAppliedUnsafe(IOption<Rule.IRule> rule, IToken original)
        {
            return rule.RemapAs(r => r.TryApply(original)).Press().Or(original);
        }
        private PSet<string> _labels;
    }
}
namespace FourZeroOne.Proxy.Unsafe
{
    using ResObj = Resolution.IResolution;
    using Token.Unsafe;
    using Token;

    public interface IProxy
    {
        public IPSet<string> Labels { get; }
        public IToken UnsafeRealize(IToken original, IOption<Rule.IRule> rule);
        public IProxy UnsafeWithLabels(IPSet<string> labels);
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
        public IPSet<string> HookRemovals { get => RemovedLabels.Elements; init => RemovedLabels = new() { Elements = value }; }
        protected readonly PSequence<IProxy> ArgProxies;
        protected readonly PSet<string> RemovedLabels;
        protected ThisProxy(IEnumerable<string> hookRemovals, IEnumerable<IProxy> proxies)
        {
            ArgProxies = new() { Elements = proxies };
            RemovedLabels = new() { Elements = hookRemovals };
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
            .WithHooks(original.Labels.Except(RemovedLabels.Elements));
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
            return o.WithLabels([.. o.Labels.Also(Labels)]);
        }

        protected readonly PSequence<IProxy> ArgProxies;
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