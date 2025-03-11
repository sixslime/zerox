namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class Core
{
    public static Korssas.Multi.Create<R> kMultiOf<R>(params IKorssa<R>[] korssas)
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

    public static Korssas.Multi.GetIndex<R> kGetIndex<R>(this IKorssa<IMulti<R>> korssa, IKorssa<Number> index)
        where R : class, Rog =>
        new(korssa, index);

    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, Multi<ROut>> kMap<RIn, ROut>(this IKorssa<IMulti<RIn>> source, IEnumerable<Addr> captures, Func<DynamicAddress<RIn>, IKorssa<ROut>> mapFunction)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.Map<RIn, ROut>.Construct(source, Core.kMetaFunction(captures, mapFunction));

    public static Korvessa<R, Number, Multi<R>> kDuplicate<R>(this IKorssa<R> value, IKorssa<Number> count)
        where R : class, Rog =>
        Korvessas.Duplicate<R>.Construct(value, count);
}