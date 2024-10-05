using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FourZeroOne.Core.Macros
{
    using Token;
    using ResObj = Resolution.IResolution;
    using Macro;
    using t = Core.Tokens;
    using r = Core.Resolutions;
    using FourZeroOne.Proxy;
    using FourZeroOne.Core.Resolutions.Actions.Board.Unit;
    using ProxySyntax;

    public sealed record Map<RIn, ROut> : OneArg<Resolution.IMulti<RIn>, r.Multi<ROut>>
        where RIn : class, ResObj
        where ROut : class, ResObj
    {
        
    }

    public sealed record Anonymous<RArg1, ROut> : OneArg<RArg1, ROut>
        where RArg1 : class, ResObj
        where ROut : class, ResObj
    {
        public required IProxy<Anonymous<RArg1, ROut>, ROut> Proxy { get; init; }
        public Anonymous(IToken<RArg1> arg1) : base(arg1) { }
    }
}