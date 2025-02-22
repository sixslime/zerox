using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixShaded.FourZeroOne.Core.Macros
{
    public static class CatchNolla<R>
        where R : Res
    {
        public static Macro<R, MetaFunction<R>, R> Construct(IToken<R> value, IToken<MetaFunction<R>> fallback) => new(value, fallback)
        {
            Label = Package.Label("CatchNolla"),
            Definition = Core.tMetaFunction<R, MetaFunction<R>, R>(
                (valueI, fallbackI) =>
                    valueI.tRef().tExists().tIfTrueDirect<R>(new()
                    {
                        Then = valueI.tRef().tMetaBoxed(),
                        Else = fallbackI.tRef()
                    })
                    .tExecute())
                .Resolution
        };
    }
}
