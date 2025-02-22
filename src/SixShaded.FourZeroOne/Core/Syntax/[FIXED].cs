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
        
        public static Tokens.Fixed<Bool> tFixed(this bool value)
        { return new(value); }
        public static Tokens.Fixed<Number> tFixed(this int value)
        { return new(value); }
        public static Tokens.Fixed<NumRange> tFixed(this Range value)
        { return new(value); }
        public static Tokens.Fixed<R> tFixed<R>(this R value) where R : class, Res
        { return new(value); }
        public static Tokens.Fixed<r.Multi<R>> tFixed<R>(this IEnumerable<R> values) where R : class, Res
        { return new(new() { Values = values.ToPSequence() }); }

    }
}
