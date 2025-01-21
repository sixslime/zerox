using System;
using ResObj = FourZeroOne.Resolution.IResolution;
using Perfection;
#nullable enable
// A 'Rule' is just a wrapper around a proxy that can 'try' to apply it.
namespace FourZeroOne.Rule
{
    using Proxy;
    using Token;
    
    public interface IRule
    {
        public IOption<IToken<R>> TryApplyTyped<R>(IToken<R> original) where R : class, ResObj;
        public IOption<Token.Unsafe.IToken> TryApply(Token.Unsafe.IToken original);
    }

    /// <summary>
    /// Rule will apply to tokens that match all of the following criteria: <br></br>
    /// - Can be assigned to type <typeparamref name="TFor"/>. (Proxy input will be <typeparamref name="TFor"/>) <br></br>
    /// - Has a resolution type that <typeparamref name="R"/> can be assigned too. (Proxy output will resolve to <typeparamref name="R"/>) <br></br>
    /// - Contains the rule's hook label.
    /// </summary>
    /// <typeparam name="TFor"></typeparam>
    /// <typeparam name="R"></typeparam>
    public record Rule<TFor, R> : IRule where TFor : IToken<R> where R : class, ResObj
    {
        public required IProxy<TFor, R> Proxy { get; init; }
        public required IPSet<string> RequiredLabels { get; init; }
        public IToken<R> Apply(TFor original)
        {
            return Proxy.Realize(original, this.AsSome());
        }
        private IOption<IToken<R>> TryApplyInternal(Token.Unsafe.IToken original)
        {
            // i don't see a better way. rules are just cheugy.
            return 
                original is TFor match
                && RequiredLabels.InversectedWith(match.Labels).Count == 0
                && original.GetType().FindInterfaces(InterfaceFilter, ITOKEN_TYPE)
                    .Map(x => x.GenericTypeArguments[0])
                    .HasMatch(x => typeof(R).IsAssignableTo(x))
                ? Apply(match).AsSome()
                : new None<IToken<R>>();
        }
        public IOption<Token.Unsafe.IToken> TryApply(Token.Unsafe.IToken original) => TryApplyInternal(original);
        public IOption<IToken<ROut>> TryApplyTyped<ROut>(IToken<ROut> original) where ROut : class, ResObj
        {
            // can be safely casted directly
            return TryApply(original).RemapAs(x => (IToken<ROut>)x);
        }
        private static bool InterfaceFilter(Type type, Object? comparison)
        {
            return comparison is Type c
                && type.IsGenericType
                && type.GetGenericTypeDefinition() == c;
        }
        public override string ToString()
        {
            return "rule";
        }
        private readonly static Type ITOKEN_TYPE = typeof(IToken<ResObj>).GetGenericTypeDefinition();
        
    }

}