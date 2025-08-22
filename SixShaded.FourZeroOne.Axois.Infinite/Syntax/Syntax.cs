namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using u.Anchors;
using u.Identifier;
using u.Data;
using u.Constructs.Move;
using Core = Core.Syntax.Core;
using u.Constructs;
using u.Constructs.Ability;
public static partial class Infinite
{
    public static IKorssa<IRoveggi<uGameAnchor>> Game => Core.kCompose<uGameAnchor>();
    public static IKorssa<IRoveggi<uConfigAnchor>> Configuration => Core.kCompose<uConfigAnchor>();
    public static IKorssa<IRoveggi<uPlayerIdentifier>> CurrentPlayer => Game.kRead().kGetRovi(u.uGame.CURRENT_PLAYER);
    public static IKorssa<IMulti<IRoveggi<uUnitIdentifier>>> AllUnits => Core.kAllRovedanggiKeys<uUnitIdentifier, IRoveggi<uUnitData>>();
    public static IKorssa<IMulti<IRoveggi<uPlayerIdentifier>>> AllPlayers => Core.kAllRovedanggiKeys<uPlayerIdentifier, IRoveggi<uPlayerData>>();
    public static Structure.Templates Template { get; } = new();
}

public static partial class KorssaSyntax
{
    public static Korvessa<IRoveggi<uHexIdentifier>, IRoveggi<uUnitIdentifier>, IRoveggi<uSpaceChecks>> kMovePathChecks(this IKorssa<IRoveggi<uHexIdentifier>> hex, IKorssa<IRoveggi<uUnitIdentifier>> unit) => Korvessas.DoMovePathChecks.Construct(hex, unit);
    public static Korvessa<IRoveggi<uHexIdentifier>, IRoveggi<uUnitIdentifier>, IRoveggi<uSpaceChecks>> kMoveDestinationChecks(this IKorssa<IRoveggi<uHexIdentifier>> hex, IKorssa<IRoveggi<uUnitIdentifier>> unit) => Korvessas.DoMoveDestinationChecks.Construct(hex, unit);
    public static Korvessa<IRoveggi<uPlayerIdentifier>, Bool> kIsBaseProtected(this IKorssa<IRoveggi<uPlayerIdentifier>> player) => Korvessas.IsBaseProtected.Construct(player);
    public static Korvessa<IRoveggi<uSourcedAbility>, IRoveggi<uUnitIdentifier>, IRoveggi<uSourceChecks>> kSourceChecks(this IKorssa<IRoveggi<uSourcedAbility>> ability, IKorssa<IRoveggi<uUnitIdentifier>> unit) => Korvessas.DoSourceChecks.Construct(ability, unit);
    public static Korvessa<IRoveggi<uMove>, IRoveggi<uUnitIdentifier>, IRoveggi<uSubjectChecks>> kMoveSubjectChecks(this IKorssa<IRoveggi<uMove>> move, IKorssa<IRoveggi<uUnitIdentifier>> unit) => Korvessas.DoMoveSubjectChecks.Construct(move, unit);
    public static Korvessa<IRoveggi<uSourcedAbility>, IRoveggi<uUnitIdentifier>, IRoveggi<uUnitIdentifier>, IRoveggi<uTargetChecks>> kTargetChecks(this IKorssa<IRoveggi<uSourcedAbility>> ability, IKorssa<IRoveggi<uUnitIdentifier>> unit, IKorssa<IRoveggi<uUnitIdentifier>> source) => Korvessas.DoTargetChecks.Construct(ability, unit, source);

    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, MetaFunction<ROut>, ROut> kIOSelectOneCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, Structure.SelectCancellable<RIn, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.SelectOneCancellable<RIn, ROut>.Construct(pool, Core.kMetaFunction([], block.Select), Core.kMetaFunction([], block.Cancel));

    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, MetaFunction<ROut>, ROut> kIOSelectOneCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, CoreStructure.Hint<ROut> hint, Structure.SelectCancellable<RIn, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        kIOSelectOneCancellable(pool, block);
    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, MetaFunction<ROut>, ROut> kIOSelectOneCancellableDirect<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, Structure.SelectCancellableDirect<RIn, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.SelectOneCancellable<RIn, ROut>.Construct(pool, block.Select, block.Cancel);

    public static Korvessa<IMulti<RIn>, MetaFunction<RIn, ROut>, MetaFunction<ROut>, ROut> kIOSelectOneCancellableDirect<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, CoreStructure.Hint<ROut> hint, Structure.SelectCancellableDirect<RIn, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        kIOSelectOneCancellableDirect(pool, block);
    public static IKorssa<ROut> kIOSelectMultipleCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, IKorssa<NumRange> count, Structure.SelectCancellable<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.SelectMultipleCancellable<RIn, ROut>.Construct(pool, Core.kMetaFunction([], block.Select), Core.kMetaFunction([], block.Cancel))
            .kExecuteWith(new()
            {
                A = count
            });

    public static IKorssa<ROut> kIOSelectMultipleCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, CoreStructure.Hint<ROut> hint, IKorssa<NumRange> count, Structure.SelectCancellable<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        kIOSelectMultipleCancellable(pool, count, block);
    public static IKorssa<ROut> kIOSelectMultipleCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, IKorssa<Number> count, Structure.SelectCancellable<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.SelectMultipleCancellable<RIn, ROut>.Construct(pool, Core.kMetaFunction([], block.Select), Core.kMetaFunction([], block.Cancel))
            .kExecuteWith(new()
            {
                A = count.kSingleRange()
            });

    public static IKorssa<ROut> kIOSelectMultipleCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, CoreStructure.Hint<ROut> hint, IKorssa<Number> count, Structure.SelectCancellable<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        kIOSelectMultipleCancellable(pool, count, block);
    public static IKorssa<ROut> kIOSelectMultipleCancellableDirect<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, IKorssa<NumRange> count, Structure.SelectCancellableDirect<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        Korvessas.SelectMultipleCancellable<RIn, ROut>.Construct(pool, block.Select, block.Cancel)
            .kExecuteWith(new()
            {
                A = count
            });

    public static IKorssa<ROut> kIOSelectMultipleCancellableDirect<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, CoreStructure.Hint<ROut> hint, IKorssa<NumRange> count, Structure.SelectCancellableDirect<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        kIOSelectMultipleCancellableDirect(pool, count, block);

    public static Korvessa<IRoveggi<uUnitIdentifier>, NumRange, NumRange> kGetMoveRange(this IKorssa<IRoveggi<uUnitIdentifier>> unit, IKorssa<NumRange> moveRange) => Korvessas.GetUnitNumericalMoveRange.Construct(unit, moveRange);
}