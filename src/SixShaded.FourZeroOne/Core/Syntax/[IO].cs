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
        public static Tokens.IO.Select.One<R> tIOSelectOne<R>(this IToken<IMulti<R>> source) where R : class, Res
        { return new(source); }
        public static Tokens.IO.Select.Multiple<R> tIOSelectMany<R>(this IToken<IMulti<R>> source, IToken<Number> count) where R : class, Res
        { return new(source, count); }
    }
}
