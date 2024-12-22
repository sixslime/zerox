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
        // do we even need hooks man

        /* we aint even need this
        public sealed record SendAction<A> : OneArg<ICompositionOf<A>, r.Multi<ResObj>> where A : class, ar.Action.IAction<A>, new()
        {
            public SendAction(IToken<ICompositionOf<A>> action) : base(action) { }
            protected override IProxy<r.Multi<ResObj>> InternalProxy => PROXY;
            public readonly static IProxy<SendAction<A>, r.Multi<ResObj>> PROXY =
                MakeProxy.Statement<SendAction<A>, r.Multi<ResObj>>(P => P.pOriginalA().pDecompose());
        }
        */
        
    }
        // numericalmove(Units, Range, Func<Hex, Hex> modifier)
        //                               ^ Glorping crazy!
    // IField<Unit, ro.Number>
    // DEV - instead of changing the checks for the pathfinding, change the hex that is checked!!
    // DEV - ITS ALL COMPONENTS !!!!!!
    // ITS ALL FUNCTIONAL !!!!!
}