using System;
using ResObj = FourZeroOne.Resolution.IResolution;
using Perfection;
#nullable enable
// A 'Rule' is just a wrapper around a proxy that can 'try' to apply it.
namespace FourZeroOne.Rule
{
    using Proxy;
    
    public interface IRule
    {
        public IOption<Token.IToken<R>> TryApplyTyped<R>(Token.IToken<R> original) where R : class, ResObj;
        public IOption<Token.Unsafe.IToken> TryApply(Token.Unsafe.IToken original);
    }

    public record Rule<TFor, R> : IRule where TFor : Token.IToken<R> where R : class, ResObj
    {
        public Rule(string hook, IProxy<TFor, R> proxy)
        {
            _proxy = proxy;
            _hook = hook;
        }
        public Token.IToken<R> Apply(TFor original)
        {
            return _proxy.Realize(original, this.AsSome());
        }
        public IOption<Token.Unsafe.IToken> TryApply(Token.Unsafe.IToken original)
        {
            return (original is TFor match && match.HookLabels.Contains(_hook)) ? Apply(match).AsSome() : original.None();
        }
        public IOption<Token.IToken<ROut>> TryApplyTyped<ROut>(Token.IToken<ROut> original) where ROut : class, ResObj
        {
            return (original is TFor match && match.HookLabels.Contains(_hook)) ? ((Token.IToken<ROut>)Apply(match)).AsSome() : original.None();
        }
        private readonly IProxy<TFor, R> _proxy;
        private readonly string _hook;
    }

}