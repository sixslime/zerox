
using Perfection;
using System.Collections.Generic;
namespace FourZeroOne.Libraries.Axiom.TokenSyntax
{
    using r = Core.Resolutions;
    using t = Core.Tokens;
    using ResObj = Resolution.IResolution;
    using IToken = Token.Unsafe.IToken;


    public static class AxiomT
    {
        
        public static t.Fixed<Components.Unit.Effects.Slow.Component> tEffectSlow()
        { return new(new()); }
        public static t.Fixed<Components.Unit.Effects.Slow.Identifier> tEffectSlowCI()
        { return new(new()); }
    }
    public static class _Extensions
    {
        
    }
}