namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class Core
{
    public static Korssas.Multi.Union<R> tMultiOf<R>(List<IKorssa<R>> korssas) where R : class, Rog => new(korssas.Map(x => x.tYield()));

    public static Korssas.Multi.Union<R> tUnionOf<R>(List<IKorssa<IMulti<R>>> sets) where R : class, Rog => new(sets);

    public static Korssas.Multi.Intersection<R> tIntersectionOf<R>(List<IKorssa<IMulti<R>>> sets) where R : class, Rog => new(sets);
}

public static partial class KorssaSyntax
{
    public static Korssas.Multi.Exclusion<R> tWithout<R>(this IKorssa<IMulti<R>> source, IKorssa<IMulti<R>> exclude) where R : class, Rog => new(source, exclude);

    public static Korssas.Multi.Count tCount(this IKorssa<IMulti<Rog>> source) => new(source);

    public static Korssas.Multi.Yield<R> tYield<R>(this IKorssa<R> korssa) where R : class, Rog => new(korssa);

    public static Korssas.Multi.Union<R> tToMulti<R>(this IEnumerable<IKorssa<R>> korssas) where R : class, Rog => new(korssas.Map(x => x.tYield()));

    public static Korssas.Multi.Union<R> tUnion<R>(this IKorssa<IMulti<R>> left, IKorssa<IMulti<R>> right) where R : class, Rog => new(left, right);

    public static Korssas.Multi.Union<R> tFlatten<R>(this IEnumerable<IKorssa<IMulti<R>>> korssas) where R : class, Rog => new(korssas);

    public static Korssas.Multi.Contains<R> tContains<R>(this IKorssa<IMulti<R>> from, IKorssa<R> element) where R : class, Rog => new(from, element);

    public static Korssas.Multi.GetIndex<R> tGetIndex<R>(this IKorssa<IMulti<R>> korssa, IKorssa<Number> index) where R : class, Rog => new(korssa, index);

    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>> tMap<RIn, ROut>(this IKorssa<IMulti<RIn>> source, Func<DynamicAddress<RIn>, IKorssa<ROut>> mapFunction)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.Map<RIn, ROut>.Construct(source, Core.tMetaFunction(mapFunction));

    public static Korvessa<R, Number, Multi<R>> tDuplicate<R>(this IKorssa<R> value, IKorssa<Number> count)
        where R : class, Rog =>
        Korvessas.Duplicate<R>.Construct(value, count);
}