
using Perfection;
using System.Collections.Generic;
namespace FourZeroOne.Libraries.Axiom.TokenSyntax
{
    using r = Core.Resolutions;
    using ro = Core.Resolutions.Objects;
    using t = Core.Tokens;
    using ResObj = Resolution.IResolution;
    using Token;
    using aEffects = Components.Unit.Effects;
    using Core.TokenSyntax;

    public static class AxiomT
    {
        
        public static t.Fixed<aEffects.Slow.Component> tEffectSlow()
        { return new(new()); }
        public static t.Fixed<aEffects.Slow.Identifier> tEffectSlowCI()
        { return new(new()); }
    }
    public static class _Extensions
    {

    }
}