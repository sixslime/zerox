namespace SixShaded.FourZeroOne.Core.Syntax;

using Roggis;
using Korvessa.Defined;

public static partial class Core
{
    public static Korssas.SubEnvironment<R> kSubEnvironment<R>(Structure.Korssa.SubEnvironment<R> block)
        where R : class, Rog =>
        new(block.Environment.kToMulti(), block.Value);
    public static Korvessa<IMulti<MetaFunction<R>>, R> kSelector<R>(List<Func<IKorssa<R>>> statements)
        where R : class, Rog =>
        Korvessas.Switch<R>.Construct(kMulti([..statements.Map(x => x().kMetaBoxed([]))]));
}

public static partial class KorssaSyntax
{

    public static Korssas.IfElse<R> kIfTrue<R>(this IKorssa<Bool> condition, Structure.Korssa.IfElse<R> block)
        where R : class, Rog =>
        new(condition, block.Then.kMetaBoxed([]), block.Else.kMetaBoxed([]));



    public static Korssas.SubEnvironment<ROut> ksSelectorExpr<RIn, ROut>(this IKorssa<RIn> val, Func<DynamicRoda<RIn>, List<Func<IKorssa<ROut>>>> relativeStatements)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Core.kSubEnvironment<ROut>(
        new()
        {
            Environment = [val.kAsVariable(out var iVal)],
            Value = Core.kSelector<ROut>(relativeStatements(iVal))
        });

    public static Korssas.SubEnvironment<ROut> ksSelectorExpr<RIn, ROut>(this IKorssa<RIn> val, Structure.Hint<ROut> resultHint, Func<DynamicRoda<RIn>, List<Func<IKorssa<ROut>>>> relativeStatements)
        where RIn : class, Rog
        where ROut : class, Rog =>
        ksSelectorExpr(val, relativeStatements);
}