using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixShaded.FourZeroOne.Core.Syntax
{
    using Resolutions;
    public static partial class Core
    {
        public static Tokens.Multi.Union<R> tMultiOf<R>(List<IToken<R>> tokens) where R : class, Res
        { return new(tokens.Map(x => x.tYield())); }
        public static Tokens.Multi.Union<R> tUnionOf<R>(List<IToken<IMulti<R>>> sets) where R : class, Res
        { return new(sets); }
        public static Tokens.Multi.Intersection<R> tIntersectionOf<R>(List<IToken<IMulti<R>>> sets) where R : class, Res
        { return new(sets); }

    }
    public static partial class TokenSyntax
    {
        public static Tokens.Multi.Exclusion<R> tWithout<R>(this IToken<IMulti<R>> source, IToken<IMulti<R>> exclude) where R : class, Res
        { return new(source, exclude); }
        public static Tokens.Multi.Count tCount(this IToken<IMulti<Res>> source)
        { return new(source); }
        public static Tokens.Multi.Yield<R> tYield<R>(this IToken<R> token) where R : class, Res
        { return new(token); }
        public static Tokens.Multi.Union<R> tToMulti<R>(this IEnumerable<IToken<R>> tokens) where R : class, Res
        { return new(tokens.Map(x => x.tYield())); }
        public static Tokens.Multi.Union<R> tUnion<R>(this IToken<IMulti<R>> left, IToken<IMulti<R>> right) where R : class, Res
        { return new([left, right]); }
        public static Tokens.Multi.Union<R> tFlatten<R>(this IEnumerable<IToken<IMulti<R>>> t) where R : class, Res
        { return new(t); }
        public static Tokens.Multi.Contains<R> tContains<R>(this IToken<IMulti<R>> from, IToken<R> element) where R : class, Res
        { return new(from, element); }
        public static Tokens.Multi.GetIndex<R> tAtIndex<R>(this IToken<IMulti<R>> token, IToken<Number> index) where R : class, Res
        { return new(token, index); }
        public static Macro<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>> tMap<RIn, ROut>(this IToken<IMulti<RIn>> source, Func<DynamicAddress<RIn>, IToken<ROut>> mapFunction)
            where RIn : class, Res
            where ROut : class, Res
        { return Macros.Map<RIn, ROut>.Construct(source, Core.tMetaFunction(mapFunction)); }
        public static Macro<R, Number, Multi<R>> tDuplicate<R>(this IToken<R> value, IToken<Number> count)
            where R : class, Res
        { return Macros.Duplicate<R>.Construct(value, count); }

    }
}
