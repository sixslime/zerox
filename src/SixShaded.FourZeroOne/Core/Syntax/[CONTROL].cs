namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class Core
{
    public static Korssas.SubEnvironment<R> kSubEnvironment<R>(Structure.Korssa.SubEnvironment<R> block)
        where R : class, Rog =>
        new(block.Environment.kToMulti(), block.Value);
}

public static partial class KorssaSyntax
{

    public static Korssas.IfElse<R> kIfTrue<R>(this IKorssa<Bool> condition, Structure.Korssa.IfElse<R> block)
        where R : class, Rog =>
        new(condition, block.Then.kMetaBoxed([]), block.Else.kMetaBoxed([]));

    public static Korvessa<RIn, IMulti<MetaFunction<RIn, Bool>>, IMulti<MetaFunction<ROut>>, ROut> kSwitch<RIn, ROut>(this IKorssa<RIn> value, List<KeyValuePair<Func<DynamicRoda<RIn>, IKorssa<Bool>>, IKorssa<ROut>>> matchPairs)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.Switch<RIn, ROut>.Construct(
        value,
        Core.kMulti(matchPairs.Map(x => Core.kMetaFunction([], x.Key))),
        Core.kMulti(matchPairs.Map(x => x.Value.kMetaBoxed([]))));

    public static Korvessa<RIn, IMulti<MetaFunction<RIn, Bool>>, IMulti<MetaFunction<ROut>>, ROut> kSwitch<RIn, ROut>(this IKorssa<RIn> value, Structure.Hint<ROut> resultHint, List<KeyValuePair<Func<DynamicRoda<RIn>, IKorssa<Bool>>, IKorssa<ROut>>> matchPairs)
        where RIn : class, Rog
        where ROut : class, Rog =>
        kSwitch(value, matchPairs);

}