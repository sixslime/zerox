using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixShaded.FourZeroOne.Core.Macros
{
    using Resolutions;
    using Syntax;

    public static class Duplicate<R>
        where R : class, Res
    {
        public static Macro<R, Number, Multi<R>> Construct(IToken<R> value, IToken<Number> count) => new(value, count)
        {
            Label = Package.Label("Duplicate"),
            Definition = Core.tMetaFunction<R, Number, Multi<R>>(
                (valueI, countI) =>
                    Core.tMetaRecursiveFunction<Number, Multi<R>>(
                    (selfFunc, i) =>
                        i.tRef().tIsGreaterThan(countI.tRef())
                        .t_IfTrue<Multi<R>>(new()
                        {
                            Then = Core.tNollaFor<Multi<R>>(),
                            Else = Core.tUnionOf<R>(
                            [
                                valueI.tRef().tYield(),
                                    selfFunc.tRef().tExecuteWith(new() { A = i.tRef().tAdd(1.tFixed()) })
                            ])
                        }))
                    .tExecuteWith(new() { A = 1.tFixed() }))
                .Resolution
        };
    }
}
