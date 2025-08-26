namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class Core
{
    [Obsolete("use kMulti")]
    public static Korssas.Multi.Create<R> kMultiOld<R>(params IKorssa<R>[] korssas)
        where R : class, Rog =>
        new(korssas);
    public static Korssas.Multi.Create<R> kMulti<R>(List<IKorssa<R>> korssas)
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

    public static Korssas.Multi.Clean<R> kClean<R>(this IKorssa<IMulti<R>> source)
        where R : class, Rog =>
        new(source);

    public static Korssas.Multi.IndiciesOf<R> kIndiciesOf<R>(this IKorssa<IMulti<R>> source, IKorssa<R> element)
        where R : class, Rog =>
        new(source, element);

    public static Korssas.Multi.Distinct<R> kDistinct<R>(this IKorssa<IMulti<R>> source)
        where R : class, Rog =>
        new(source);

    public static Korssas.Multi.Union<R> kUnion<R>(this IKorssa<IMulti<R>> a, IKorssa<IMulti<R>> b)
        where R : class, Rog =>
        new(a, b);

    public static Korssas.Multi.Intersect<R> kIntersect<R>(this IKorssa<IMulti<R>> a, IKorssa<IMulti<R>> b)
        where R : class, Rog =>
        new(a, b);

    public static Korssas.Multi.Except<R> kExcept<R>(this IKorssa<IMulti<R>> a, IKorssa<IMulti<R>> b)
        where R : class, Rog =>
        new(a, b);

    public static Korvessas.AllMatch<R> kAllMatch<R>(this IKorssa<IMulti<R>> source, IKorssa<MetaFunction<R, Bool>> predicate)
        where R : class, Rog =>
        new(source, predicate);

    public static Korvessas.AllMatch<R> kAllMatch<R>(this IKorssa<IMulti<R>> source, MetaDefinition<R, Bool> predicate)
        where R : class, Rog =>
        new(source, Core.kMetaFunction([], predicate));

    public static Korvessas.Accumulate<RIn, ROut> kAccumulateInto<RIn, ROut>(this IKorssa<IMulti<RIn>> source, IKorssa<ROut> initialValue, IKorssa<MetaFunction<ROut, RIn, ROut>> function)
        where RIn : class, Rog
        where ROut : class, Rog =>
        new(source, initialValue, function);

    public static Korvessas.Accumulate<RIn, ROut> kAccumulateInto<RIn, ROut>(this IKorssa<IMulti<RIn>> source, IKorssa<ROut> initialValue, MetaDefinition<ROut, RIn, ROut> function)
        where RIn : class, Rog
        where ROut : class, Rog =>
        new(source, initialValue, Core.kMetaFunction([], function));

    public static Korvessas.AnyMatch<R> kAnyMatch<R>(this IKorssa<IMulti<R>> source, IKorssa<MetaFunction<R, Bool>> predicate)
        where R : class, Rog =>
        new(source, predicate);

    public static Korvessas.AnyMatch<R> kAnyMatch<R>(this IKorssa<IMulti<R>> source, MetaDefinition<R, Bool> predicate)
        where R : class, Rog =>
        new(source, Core.kMetaFunction([], predicate));

    public static Korvessas.DistinctBy<R> kDistinctBy<R>(this IKorssa<IMulti<R>> source, MetaDefinition<R, Rog> keyFunction)
        where R : class, Rog =>
        new(source, Core.kMetaFunction([], keyFunction));

    public static Korvessas.DistinctBy<R> kDistinctBy<R>(this IKorssa<IMulti<R>> source, IKorssa<MetaFunction<R, Rog>> keyFunction)
        where R : class, Rog =>
        new(source, keyFunction);

    public static Korvessas.Map<RIn, ROut> kMap<RIn, ROut>(this IKorssa<IMulti<RIn>> source, MetaDefinition<RIn, ROut> mapFunction)
        where RIn : class, Rog
        where ROut : class, Rog =>
        new(source, Core.kMetaFunction([], mapFunction));

    public static Korvessas.Filter<R> kWhere<R>(this IKorssa<IMulti<R>> source, IKorssa<MetaFunction<R, Bool>> predicate)
        where R : class, Rog =>
        new(source, predicate);

    public static Korvessas.Filter<R> kWhere<R>(this IKorssa<IMulti<R>> source, MetaDefinition<R, Bool> predicate)
        where R : class, Rog =>
        new(source, Core.kMetaFunction([], predicate));

    public static Korvessas.FirstMatch<R> kFirstMatch<R>(this IKorssa<IMulti<R>> source, IKorssa<MetaFunction<R, Bool>> predicate)
        where R : class, Rog =>
        new(source, predicate);

    public static Korvessas.FirstMatch<R> kFirstMatch<R>(this IKorssa<IMulti<R>> source, MetaDefinition<R, Bool> predicate)
        where R : class, Rog =>
        new(source, Core.kMetaFunction([], predicate));

    public static Korvessas.Sequence<R> kGenerateSequence<R>(this IKorssa<R> initialValue, MetaDefinition<R, Number, R> generator)
        where R : class, Rog =>
        new(initialValue, Core.kMetaFunction([], generator));

    public static Korvessas.Sequence<R> kGenerateSequence<R>(this IKorssa<R> initialValue, IKorssa<MetaFunction<R, Number, R>> generator)
        where R : class, Rog =>
        new(initialValue, generator);

    public static Korvessas.Map<RIn, ROut> kMap<RIn, ROut>(this IKorssa<IMulti<RIn>> source, IKorssa<MetaFunction<RIn, ROut>> mapFunction)
        where RIn : class, Rog
        where ROut : class, Rog =>
        new(source, mapFunction);

    public static Korvessas.MapWithIndex<RIn, ROut> kMapWithIndex<RIn, ROut>(this IKorssa<IMulti<RIn>> source, MetaDefinition<RIn, Number, ROut> mapFunction)
        where RIn : class, Rog
        where ROut : class, Rog =>
        new(source, Core.kMetaFunction([], mapFunction));

    public static Korvessas.MapWithIndex<RIn, ROut> kMapWithIndex<RIn, ROut>(this IKorssa<IMulti<RIn>> source, IKorssa<MetaFunction<RIn, Number, ROut>> mapFunction)
        where RIn : class, Rog
        where ROut : class, Rog =>
        new(source, mapFunction);

    public static Korvessas.Concat<R> kConcat<R>(this IKorssa<IMulti<R>> a, IKorssa<IMulti<R>> b)
        where R : class, Rog =>
        new(a, b);

    public static Korvessas.Duplicate<R> kDuplicate<R>(this IKorssa<R> value, IKorssa<Number> count)
        where R : class, Rog =>
        new(value, count);
}
