using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixShaded.FourZeroOne.Core.Syntax
{
    using Resolutions;
    public static partial class TokenSyntax
    {
        public static Macro<R, MetaFunction<R>, R> tCatchNolla<R>(this IToken<R> value, IToken<MetaFunction<R>> fallback)
            where R : class, Res
        { return Macros.CatchNolla<R>.Construct(value, fallback); }
        public static Macro<R, MetaFunction<R>, R> tCatchNolla<R>(this IToken<R> value, Func<IToken<R>> fallback)
            where R : class, Res
        { return Macros.CatchNolla<R>.Construct(value, fallback().tMetaBoxed()); }
        public static Tokens.Exists tExists(this IToken<Res> token)
        { return new(token); }
        public static Tokens.Nolla<R> tNollaFor<R>() where R : class, Res
        { return new(); }

    }
}
