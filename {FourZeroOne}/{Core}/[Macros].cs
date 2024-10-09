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
    using TokenSyntax;


    public sealed record Map<RIn, ROut> : TwoArg<Resolution.IMulti<RIn>, r.Boxed.MetaFunction<RIn, ROut>, r.Multi<ROut>>
        where RIn : class, ResObj
        where ROut : class, ResObj
    {
        public Map(IToken<Resolution.IMulti<RIn>> values, IToken<r.Boxed.MetaFunction<RIn, ROut>> mapFunction) : base(values, mapFunction) { }
        private static IProxy<Map<RIn, ROut>, r.Multi<ROut>> _proxy = CoreP.Statement<Map<RIn, ROut>, r.Multi<ROut>>(P =>
        {
            return CoreT.tRecursive<>
        });
    }
}