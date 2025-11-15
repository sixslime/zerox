namespace SixShaded.FourZeroOne.Axois.Infinite.Syntax;

using Korvessas.Game;
using u.Anchors;
using u.Identifier;
using u.Data;
using u.Constructs.Move;
using Core = Core.Syntax.Core;
using u.Constructs;
using u.Constructs.Ability;
using u.Constructs.Resolved;
using u.Config;
using u.Constructs.GameResults;
using Korvessas;

public static partial class Infinite
{
    public static IKorssa<IRoveggi<uGameAnchor>> Game => Core.kCompose<uGameAnchor>();
    public static IKorssa<IRoveggi<uConfigAnchor>> Configuration => Core.kCompose<uConfigAnchor>();
    public static IKorssa<IRoveggi<uPlayerIdentifier>> CurrentPlayer => Game.kRead().kGetRovi(u.uGame.CURRENT_PLAYER);
    public static IKorssa<IMulti<IRoveggi<uUnitIdentifier>>> AllUnits => Core.kAllRovedanggiKeys<uUnitIdentifier, IRoveggi<uUnitData>>();
    public static IKorssa<IMulti<IRoveggi<uPlayerIdentifier>>> AllPlayers => Core.kAllRovedanggiKeys<uPlayerIdentifier, IRoveggi<uPlayerData>>();
    public static Structure.Templates Template { get; } = new();

    public static IKorssa<IRoveggi<uGameResult>> Main() =>
        Core.kSubEnvironment<IRoveggi<uGameResult>>(
        new()
        {
            Environment = [kDoSetupGame(Core.kCompose<uConfigAnchor>().kRead())],
            Value = kGameLoop(),
        });

    public static GameLoop kGameLoop() => new();
    public static DoTurnCycle kDoCycleTurnOrder() => new();
    public static GameResultCheck kCheckGameResult() => new();
    public static SetupGame kDoSetupGame(IKorssa<IRoveggi<uGameConfiguration>> config) => new(config);
    public static DoStandardTurn kDoStandardTurn(IKorssa<IRoveggi<uPlayerIdentifier>> player) => new(player);
}

public static partial class KorssaSyntax
{
    public static GameProgressionCheck kHasGameProgressedSince(this IKorssa<ProgramState> state) => new(state);
    public static DoEliminateUnit kDoEliminate(this IKorssa<IRoveggi<uUnitIdentifier>> unit) => new(unit);
    public static DoUnitEffectCycle kDoUnitEffectCycle(this IKorssa<IRoveggi<uPlayerIdentifier>> player) => new(player);
    public static AllowPlay kAllowPlay(this IKorssa<IRoveggi<uPlayerIdentifier>> player) => new(player);
    public static RestockPlayerHand kWithRestockedHand(this IKorssa<IRoveggi<uPlayerData>> playerData) => new(playerData);
    public static RefreshPlayerEnergy kWithRefreshedEnergy(this IKorssa<IRoveggi<uPlayerData>> playerData) => new(playerData);
    public static CheckWin kWinChecks(this IKorssa<IRoveggi<uPlayerIdentifier>> player) => new(player);
    public static CheckMovePath kMovePathChecks(this IKorssa<IRoveggi<uHexIdentifier>> hex, IKorssa<IRoveggi<uUnitIdentifier>> unit) => new(hex, unit);
    public static CheckMoveDestination kMoveDestinationChecks(this IKorssa<IRoveggi<uHexIdentifier>> hex, IKorssa<IRoveggi<uUnitIdentifier>> unit) => new(hex, unit);
    public static IsBaseProtected kIsBaseProtected(this IKorssa<IRoveggi<uPlayerIdentifier>> player) => new(player);
    public static CheckAbilitySource kSourceChecks(this IKorssa<IRoveggi<uSourcedAbility>> ability, IKorssa<IRoveggi<uUnitIdentifier>> unit) => new(ability, unit);
    public static CheckMoveSubject kMoveSubjectChecks(this IKorssa<IRoveggi<uMove>> move, IKorssa<IRoveggi<uUnitIdentifier>> unit) => new(move, unit);
    public static CheckAbilityTarget kTargetChecks(this IKorssa<IRoveggi<uSourcedAbility>> ability, IKorssa<IRoveggi<uUnitIdentifier>> unit, IKorssa<IRoveggi<uUnitIdentifier>> source) => new(ability, unit, source);

    public static SelectOneCancellable<RIn, ROut> kIOSelectOneCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, Structure.SelectCancellable<RIn, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        new(pool, Core.kMetaFunction([], block.Select), Core.kMetaFunction([], block.Cancel));

    public static SelectOneCancellable<RIn, ROut> kIOSelectOneCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, CoreStructure.Hint<ROut> hint, Structure.SelectCancellable<RIn, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        kIOSelectOneCancellable(pool, block);

    public static SelectOneCancellable<RIn, ROut> kIOSelectOneCancellableDirect<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, Structure.SelectCancellableDirect<RIn, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        new(pool, block.Select, block.Cancel);

    public static SelectOneCancellable<RIn, ROut> kIOSelectOneCancellableDirect<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, CoreStructure.Hint<ROut> hint, Structure.SelectCancellableDirect<RIn, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        kIOSelectOneCancellableDirect(pool, block);

    public static IKorssa<ROut> kIOSelectMultipleCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, IKorssa<NumRange> count, Structure.SelectCancellable<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        new SelectMultipleCancellable<RIn, ROut>(pool, Core.kMetaFunction([], block.Select), Core.kMetaFunction([], block.Cancel))
            .kExecuteWith(
            new()
            {
                A = count,
            });

    public static IKorssa<ROut> kIOSelectMultipleCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, CoreStructure.Hint<ROut> hint, IKorssa<NumRange> count, Structure.SelectCancellable<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        kIOSelectMultipleCancellable(pool, count, block);

    public static IKorssa<ROut> kIOSelectMultipleCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, IKorssa<Number> count, Structure.SelectCancellable<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        new SelectMultipleCancellable<RIn, ROut>(pool, Core.kMetaFunction([], block.Select), Core.kMetaFunction([], block.Cancel))
            .kExecuteWith(
            new()
            {
                A = count.kSingleRange(),
            });

    public static IKorssa<ROut> kIOSelectMultipleCancellable<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, CoreStructure.Hint<ROut> hint, IKorssa<Number> count, Structure.SelectCancellable<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        kIOSelectMultipleCancellable(pool, count, block);

    public static IKorssa<ROut> kIOSelectMultipleCancellableDirect<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, IKorssa<NumRange> count, Structure.SelectCancellableDirect<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        new SelectMultipleCancellable<RIn, ROut>(pool, block.Select, block.Cancel)
            .kExecuteWith(
            new()
            {
                A = count,
            });

    public static IKorssa<ROut> kIOSelectMultipleCancellableDirect<RIn, ROut>(this IKorssa<IMulti<RIn>> pool, CoreStructure.Hint<ROut> hint, IKorssa<NumRange> count, Structure.SelectCancellableDirect<IMulti<RIn>, ROut> block)
        where RIn : class, Rog
        where ROut : class, Rog =>
        kIOSelectMultipleCancellableDirect(pool, count, block);

    public static GetUnitNumericalMoveRange kGetMoveRange(this IKorssa<IRoveggi<uUnitIdentifier>> unit, IKorssa<NumRange> moveRange) => new(unit, moveRange);
}
