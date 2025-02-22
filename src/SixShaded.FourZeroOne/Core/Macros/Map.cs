using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixShaded.FourZeroOne.Core.Macros
{
    public static class Map<RIn, ROut>
            where RIn : class, ResObj
            where ROut : class, ResObj
    {
        public static Macro<IMulti<RIn>, MetaFunction<RIn, ROut>, r.Multi<ROut>> Construct(IToken<IMulti<RIn>> multi, IToken<MetaFunction<RIn, ROut>> mapFunction)
        {
            return new(multi, mapFunction)
            {
                Label = Package.Label("map"),
                Definition = Core.tMetaFunction<IMulti<RIn>, MetaFunction<RIn, ROut>, r.Multi<ROut>>(
                    (multiI, mapFunctionI) =>
                        Core.tMetaRecursiveFunction<ro.Number, r.Multi<ROut>>(
                        (selfFunc, i) =>
                            i.tRef().tIsGreaterThan(multiI.tRef().tCount())
                            .t_IfTrue<r.Multi<ROut>>(new()
                            {
                                Then = Core.tNollaFor<r.Multi<ROut>>(),
                                Else = Core.tUnionOf<ROut>(
                                [
                                    mapFunctionI.tRef().tExecuteWith(
                                            new() { A = multiI.tRef().tAtIndex(i.tRef()) }).tYield(),
                                        selfFunc.tRef().tExecuteWith(
                                            new() { A = i.tRef().tAdd(1.tFixed()) })
                                ])
                            }))
                        .tExecuteWith(new() { A = 1.tFixed() }))
                    .Resolution
            };
        }
    }
}
