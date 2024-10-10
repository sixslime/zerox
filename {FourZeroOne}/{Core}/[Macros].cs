using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Perfection;

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
    using FourZeroOne.Proxy.Unsafe;

    public sealed record Map<RIn, ROut> : TwoArg<Resolution.IMulti<RIn>, r.Boxed.MetaFunction<RIn, ROut>, r.Multi<ROut>>
        where RIn : class, ResObj
        where ROut : class, ResObj
    {
        protected override IProxy<r.Multi<ROut>> InternalProxy => _proxy;
        public Map(IToken<Resolution.IMulti<RIn>> values, IToken<r.Boxed.MetaFunction<RIn, ROut>> mapFunction) : base(values, mapFunction) { }

        private static IProxy<Map<RIn, ROut>, r.Multi<ROut>> _proxy = CoreP.Statement<Map<RIn, ROut>, r.Multi<ROut>>(P =>
        {
            return 
            P.pSubEnvironment(RHint<r.Multi<ROut>>.Hint(), new()
            {
                EnvironmentProxy =
                P.pArrayOf(RHint<ResObj>.Hint(),
                [
                    P.pOriginalA().pAsVariable(out var enumerable),
                    P.pOriginalB().pAsVariable(out var mapFunction)
                ]),
                SubProxy =
                CoreT.tMetaRecursiveFunction(RHint<r.Objects.Number, r.Multi<ROut>>.Hint(), (selfFunc, i) =>
                {
                    return i.tRef().tIsGreaterThan(enumerable.tRef().tCount())
                    .tIfTrue(RHint<r.Multi<ROut>>.Hint(), new()
                    {
                        Then = CoreT.tNolla(RHint<r.Multi<ROut>>.Hint()).tMetaBoxed(),
                        Else =
                        CoreT.tUnion<ROut>(RHint<ROut>.Hint(),
                        [
                            mapFunction.tRef().tExecuteWith(new() { A = enumerable.tRef().tGetIndex(i.tRef()) }).tYield(),
                            selfFunc.tRef().tExecuteWith(new() { A = i.tRef().tAdd(1.tConst()) })
                        ]).tMetaBoxed()

                    }).tExecute();
                }).tExecuteWith(new() { A = 1.tConst() }).pDirect(P)
            });
        });
        protected override IOption<string> CustomToString() => $"{Arg1}=>{Arg2}".AsSome();
    }
    
}