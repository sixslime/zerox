using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourZeroOne.Libraries.Axiom.Macros
{
    using Token;
    using Macro;
    using ResObj = Resolution.IResolution;
    using t = Core.Tokens;
    using r = Core.Resolutions;
    using FourZeroOne.Proxy;
    using Core.Syntax;
    using ar = Resolutions;
    using ax = Resolutions.GameObjects;
    using Resolution;
    using FourZeroOne.Proxy.Unsafe;

    public sealed record SendAction<R> : OneArg<R, r.Multi<R>> where R : class, ResObj
    {
        public SendAction(IToken<R> action) : base(action) { }
        protected override IProxy<r.Multi<R>> InternalProxy => PROXY;
        private readonly static IProxy<SendAction<R>, r.Multi<R>> PROXY = ProxyStatement.Build<SendAction<R>, r.Multi<R>>(P => P.pOriginalA().pYield());
    }
    namespace GameActions
    {
        using FourZeroOne.Proxy.Unsafe;
    
        // numericalmove(Units, Range, Func<Hex, Hex> modifier)
        //                               ^ Glorping crazy!
    }
    // IField<Unit, ro.Number>
    // DEV - instead of changing the checks for the pathfinding, change the hex that is checked!!
    // DEV - ITS ALL COMPONENTS !!!!!!
    // ITS ALL FUNCTIONAL !!!!!
}