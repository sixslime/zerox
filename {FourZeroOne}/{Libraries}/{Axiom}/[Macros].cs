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

    
    namespace Hooks
    {
        // yes, this is good. this means that "gameactions" (and other action structures) are defined in resolutions
        public sealed record SendAction<R> : OneArg<R, IMulti<ResObj>> where R : class, ResObj
        {
            public SendAction(IToken<R> action) : base(action) { }
            protected override IProxy<IMulti<ResObj>> InternalProxy => PROXY;
            private readonly static IProxy<SendAction<R>, IMulti<ResObj>> PROXY = ProxyStatement.Build<SendAction<R>, IMulti<ResObj>>(P => P.pOriginalA().pYield());
        }
        
    } 
        // numericalmove(Units, Range, Func<Hex, Hex> modifier)
        //                               ^ Glorping crazy!
    // IField<Unit, ro.Number>
    // DEV - instead of changing the checks for the pathfinding, change the hex that is checked!!
    // DEV - ITS ALL COMPONENTS !!!!!!
    // ITS ALL FUNCTIONAL !!!!!
}