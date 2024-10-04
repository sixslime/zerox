
using Perfection;
using System.Collections.Generic;
namespace FourZeroOne.Libraries.Axiom.ProxySyntax
{
    using r = Core.Resolutions;
    using ro = Core.Resolutions.Objects;
    using t = Core.Tokens;
    using ResObj = Resolution.IResolution;
    using Token.Unsafe;
    using Resolution;
    using TokenSyntax;
    using Proxy;
    using Core.Proxies;
    using aEffects = Components.Unit.Effects;

    public static class AxiomP
    {
        
    }
    public static class _Extensions
    {
        public static Function<t.Number.Multiply, TOrig, ro.Number, ro.Number, ro.Number> pMultiply<TOrig>(this IProxy<TOrig, ro.Number> a, IProxy<TOrig, ro.Number> b) where TOrig : IToken
        { return new(a, b); }
        // some real bullshit i wont lie!
        public static Function<t.Component.Get<ro.Board.Unit, aEffects.Slow.Component>, TOrig, ro.Board.Unit, IComponentIdentifier<aEffects.Slow.Component>, aEffects.Slow.Component> pGetEffectSlow<TOrig>(this IProxy<TOrig, ro.Board.Unit> unit) where TOrig : IToken
        {
            return new(unit, new Direct<TOrig, aEffects.Slow.Identifier>(AxiomT.tEffectSlowCI()));
        }
    }
}