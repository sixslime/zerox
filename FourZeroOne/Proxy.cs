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
    using FourZeroOne.Proxy.Unsafe;

    public interface IProxy<in TOrig, out R> : Unsafe.IProxyOf<TOrig>, Unsafe.IProxy<R> where TOrig : IToken where R : class, ResObj
    {
        public IToken<R> Realize(TOrig original, IOption<Rule.IRule> realizingRule);
    }
    public abstract record Proxy<TOrig, R> : IProxy<TOrig, R> where TOrig : IToken where R : class, ResObj
    {
        // HookLabels are appended to ArgTransform.
        // This is a cringe :P
        public IPSet<string> Labels { get; private init; } = new PSet<string>();
        IProxy IProxy._dLabels(Updater<IPSet<string>> updater)
            => this with { Labels = updater(Labels) };
        protected abstract IToken<R> RealizeInternal(TOrig original, IOption<Rule.IRule> realizingRule);
        public IToken<R> Realize(TOrig original, IOption<Rule.IRule> realizingRule)
        {
            return RealizeInternal(original, realizingRule).dLabels(x => x.MergedWith(Labels));
        }
        public IToken<R> UnsafeTypedRealize(IToken original, IOption<Rule.IRule> rule) => Realize((TOrig)original, rule);
        public IToken UnsafeContextualRealize(TOrig original, IOption<Rule.IRule> rule) => Realize(original, rule);
        public IToken UnsafeRealize(IToken original, IOption<Rule.IRule> rule) => UnsafeTypedRealize(original, rule);
        protected static IToken<RLocal> RuleApplied<RLocal>(IOption<Rule.IRule> rule, IToken<RLocal> original) where RLocal : class, ResObj
        {
            return rule.RemapAs(r => r.TryApplyTyped(original)).Press().Or(original);
        }
        protected static IToken RuleAppliedUnsafe(IOption<Rule.IRule> rule, IToken original)
        {
            return rule.RemapAs(r => r.TryApply(original)).Press().Or(original);
        }
    }
    public static class SelfAssumptions
    {
        public static Self dLabels<Self>(this Self s, Updater<IPSet<string>> updater) where Self : Unsafe.IProxy
            => (Self)s._dLabels(updater);
        public static Self dLabelRemovals<Self>(this Self s, Updater<IPSet<string>> updater) where Self : IThisProxy
            => (Self)s._dLabelsRemovals(updater);
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
        public IProxy _dLabels(Updater<IPSet<string>> updater);
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

    public interface IThisProxy : IProxy
    {
        public IPSet<string> LabelRemovals { get; }
        public IThisProxy _dLabelsRemovals(Updater<IPSet<string>> updater);
    }
    // DEV: perhaps make FunctionProxy follow the same structure as TransformProxy
    public abstract record ThisProxy<TOrig, R> : Proxy<TOrig, R>, IThisProxy where TOrig : IToken<R> where R : class, ResObj
    {
        public IPSet<string> LabelRemovals { get; private init; } = new PSet<string>();
        // DEV: this doesn't need to be in '_d' form, but for consistency with other things it is.
        public IThisProxy _dLabelsRemovals(Updater<IPSet<string>> updater)
            => this with { LabelRemovals = updater(LabelRemovals) };
        protected readonly IProxy[] ArgProxies;
        protected ThisProxy(IEnumerable<IProxy> proxies) : this(proxies.ToArray()) { }
        protected ThisProxy(params IProxy[] proxies)
        {
            ArgProxies = proxies;
        }
        protected override IToken<R> RealizeInternal(TOrig original, IOption<Rule.IRule> rule)
        {
            return original.UnsafeTypedWithArgs(
                ArgProxies.ZipLong(original.ArgTokens)
                .Map(optPair => optPair.a.RemapAs(argProxy => argProxy.UnsafeRealize(original, rule))
                        .Or(RuleAppliedUnsafe(rule, optPair.b.Unwrap())))
                .ToArray())
            .dLabels(x => x.WithoutEntries(LabelRemovals.Elements));
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
            return o.dLabels(x => x.MergedWith(Labels));
        }

        protected readonly IProxy[] ArgProxies;
        protected abstract IToken<R> ConstructFromArgs(TOrig original, List<IToken> tokens);
        protected FunctionProxy(IEnumerable<IProxy> proxies) : this(proxies.ToArray()) { }
        protected FunctionProxy(params IProxy[] proxies)
        {
            ArgProxies = proxies;
        }
        protected List<IToken> MakeSubstitutions(TOrig original, IOption<Rule.IRule> rule)
        {
            return new(ArgProxies.Map(x => x.UnsafeRealize(original, rule)));
        }
    }
}