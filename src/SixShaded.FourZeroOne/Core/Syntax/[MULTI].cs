namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class Core
{
    public static Korssas.Multi.Create<R> kMulti<R>(params IKorssa<R>[] korssas)
        where R : class, Rog =>
        new(korssas);
    public static Korssas.Multi.Create<R> kMulti<R>(IEnumerable<IKorssa<R>> korssas)
        where R : class, Rog =>
        new(korssas);
}

public static partial class KorssaSyntax
{
    public static Korssas.Multi.Count kCount(this IKorssa<IMulti<Rog>> source) => new(source);

    public static Korssas.Multi.Yield<R> kYield<R>(this IKorssa<R> korssa)
        where R : class, Rog =>
        new(korssa);

    public static Korssas.Multi.Create<R> kToMulti<R>(this IEnumerable<IKorssa<R>> korssas)
        where R : class, Rog =>
        new(korssas.ToArray());

    public static Korssas.Multi.Exclusion<R> kExclude<R>(this IKorssa<IMulti<R>> from, IKorssa<IMulti<R>> exclude)
        where R : class, Rog =>
        new(from, exclude);

    public static Korssas.Multi.Intersection<R> kIntersection<R>(this IKorssa<IMulti<IMulti<R>>> multis)
        where R : class, Rog =>
        new(multis);

    public static Korssas.Multi.Flatten<R> kFlatten<R>(this IKorssa<IMulti<IMulti<R>>> korssas)
        where R : class, Rog =>
        new(korssas);

    public static Korssas.Multi.Contains<R> kContains<R>(this IKorssa<IMulti<R>> from, IKorssa<R> element)
        where R : class, Rog =>
        new(from, element);

    public static Korssas.Multi.GetIndex<R> kGetIndex<R>(this IKorssa<IMulti<R>> from, IKorssa<Number> index)
        where R : class, Rog =>
        new(from, index);

    public static Korssas.Multi.GetSlice<R> kGetSlice<R>(this IKorssa<IMulti<R>> from, IKorssa<NumRange> index)
        where R : class, Rog =>
        new(from, index);

    public static Korssas.Multi.Reverse<R> kReversed<R>(this IKorssa<IMulti<R>> source)
        where R : class, Rog =>
        new(source);

    public static Korvessa<IMulti<R>, Multi<R>> kDistinct<R>(this IKorssa<IMulti<R>> source)
        where R : class, Rog =>
        Korvessas.Distinct<R>.Construct(source);


    public static Korvessa<IMulti<R>, MetaFunction<R, Rog>, Multi<R>> kDistinctBy<R>(this IKorssa<IMulti<R>> source, Func<DynamicRoda<R>, Kor> keyFunction)
        where R : class, Rog =>
        Korvessas.DistinctBy<R>.Construct(source, Core.kMetaFunction([], keyFunction));
    public static Korvessa<IMulti<R>, MetaFunction<R, Rog>, Multi<R>> kDistinctBy<R>(this IKorssa<IMulti<R>> source, IKorssa<MetaFunction<R, Rog>> keyFunction)
        where R : class, Rog =>
        Korvessas.DistinctBy<R>.Construct(source, keyFunction);

    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>> kMap<RIn, ROut>(this IKorssa<IMulti<RIn>> source, Func<DynamicRoda<RIn>, IKorssa<ROut>> mapFunction)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.Map<RIn, ROut>.Construct(source, Core.kMetaFunction([], mapFunction));

    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>> kMap<RIn, ROut>(this IKorssa<IMulti<RIn>> source, IKorssa<MetaFunction<RIn, ROut>> mapFunction)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.Map<RIn, ROut>.Construct(source, mapFunction);

    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, Number, ROut>, Multi<ROut>> kMapWithIndex<RIn, ROut>(this IKorssa<IMulti<RIn>> source, Func<DynamicRoda<RIn>, DynamicRoda<Number>, IKorssa<ROut>> mapFunction)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.MapWithIndex<RIn, ROut>.Construct(source, Core.kMetaFunction([], mapFunction));

    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, Number, ROut>, Multi<ROut>> kMapWithIndex<RIn, ROut>(this IKorssa<IMulti<RIn>> source, IKorssa<MetaFunction<RIn, Number, ROut>> mapFunction)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.MapWithIndex<RIn, ROut>.Construct(source, mapFunction);

    public static Korvessa<IMulti<R>, IMulti<R>, Multi<R>> kConcat<R>(this IKorssa<IMulti<R>> a, IKorssa<IMulti<R>> b)
        where R : class, Rog =>
        Korvessas.Concat<R>.Construct(a, b);
    /*
    public static Korssas.Multi.Concat<R> kConcat<R>(this IKorssa<IMulti<R>> a, IKorssa<IMulti<R>> b)
        where R : class, Rog =>
        new(a, b);
    */
    public static Korvessa<R, Number, Multi<R>> kDuplicate<R>(this IKorssa<R> value, IKorssa<Number> count)
        where R : class, Rog =>
        Korvessas.Duplicate<R>.Construct(value, count);
}