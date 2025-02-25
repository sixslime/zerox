namespace SixShaded.FourZeroOne.Core.Syntax;

using Resolutions;
public static partial class Core
{
    public static Tokens.Multi.Union<R> tMultiOf<R>(List<IToken<R>> tokens) where R : class, Res => new(tokens.Map(x => x.tYield()));

    public static Tokens.Multi.Union<R> tUnionOf<R>(List<IToken<IMulti<R>>> sets) where R : class, Res => new(sets);

    public static Tokens.Multi.Intersection<R> tIntersectionOf<R>(List<IToken<IMulti<R>>> sets) where R : class, Res => new(sets);
}
public static partial class TokenSyntax
{
    public static Tokens.Multi.Exclusion<R> tWithout<R>(this IToken<IMulti<R>> source, IToken<IMulti<R>> exclude) where R : class, Res => new(source, exclude);

    public static Tokens.Multi.Count tCount(this IToken<IMulti<Res>> source) => new(source);

    public static Tokens.Multi.Yield<R> tYield<R>(this IToken<R> token) where R : class, Res => new(token);

    public static Tokens.Multi.Union<R> tToMulti<R>(this IEnumerable<IToken<R>> tokens) where R : class, Res => new(tokens.Map(x => x.tYield()));

    public static Tokens.Multi.Union<R> tUnion<R>(this IToken<IMulti<R>> left, IToken<IMulti<R>> right) where R : class, Res => new(left, right);

    public static Tokens.Multi.Union<R> tFlatten<R>(this IEnumerable<IToken<IMulti<R>>> tokens) where R : class, Res => new(tokens);

    public static Tokens.Multi.Contains<R> tContains<R>(this IToken<IMulti<R>> from, IToken<R> element) where R : class, Res => new(from, element);

    public static Tokens.Multi.GetIndex<R> tGetIndex<R>(this IToken<IMulti<R>> token, IToken<Number> index) where R : class, Res => new(token, index);

    public static Macro<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>> tMap<RIn, ROut>(this IToken<IMulti<RIn>> source, Func<DynamicAddress<RIn>, IToken<ROut>> mapFunction)
        where RIn : class, Res
        where ROut : class, Res =>
        Macros.Map<RIn, ROut>.Construct(source, Core.tMetaFunction(mapFunction));

    public static Macro<R, Number, Multi<R>> tDuplicate<R>(this IToken<R> value, IToken<Number> count)
        where R : class, Res =>
        Macros.Duplicate<R>.Construct(value, count);
}