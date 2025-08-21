namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas;

using Core = Core.Syntax.Core;
using u.Config;
using u;
using u.Identifier;
using u.Data;
using u.Constructs;
using u.Constructs.Resolved;
using Infinite = Syntax.Infinite;
using u.Constructs.ActionTypes;
using u.Constructs.Move;
public static class SetupGame
{
    public static Korvessa<IRoveggi<uGameConfiguration>, Rog> Construct(IKorssa<IRoveggi<uGameConfiguration>> config) =>
        new(config)
        {
            Du = Axoi.Korvedu("SetupGame"),
            Definition =
                (_, iConfig) =>
                    Core.kSubEnvironment<Rog>(
                    new()
                    {
                        Environment =
                        [
                            // player declarations:
                            iConfig.kRef()
                                .kGetRovi(uGameConfiguration.PLAYERS)
                                .kAsVariable(out var iPlayerDeclarations),

                            // player identifiers:
                            1.kFixed()
                                .kRangeTo(iPlayerDeclarations.kRef().kCount())
                                .kMap(
                                iIndex =>
                                    Core.kCompose<uPlayerIdentifier>()
                                        .kWithRovi(uPlayerIdentifier.NUMBER, iIndex.kRef()))
                                .kAsVariable(out var iPlayerIdentifiers),

                            // playable actions:
                            // silly syntax hack(s) to avoid writing a shitton of generics
                            new Func<IKorssa<IMulti<IRoveggi<uPlayableAction>>>>(
                                () =>
                                {
                                    IKorssa<IRoveggi<uPlayableAction>> kAction<C>(IKorssa<Bool> condition, IKorssa<Rog> statement)
                                        where C : uActionType, IConcreteRovetu
                                    {
                                        return Core.kCompose<uPlayableAction>()
                                            .kWithRovi(uPlayableAction.CONDITION, condition.kMetaBoxed([]))
                                            .kWithRovi(uPlayableAction.STATEMENT, statement.kMetaBoxed([]))
                                            .kWithRovi(uPlayableAction.TYPE, Core.kCompose<C>().kIsType<IRoveggi<uActionType>>());
                                    }
                                    return
                                        Core.kMulti<IRoveggi<uPlayableAction>>(
                                        new()
                                        {
                                            // ABILITY ACTION:
                                            kAction<uAbilityAction>(
                                            condition:
                                            Infinite.CurrentPlayer
                                                .kRead()
                                                .kGetRovi(uPlayerData.ENERGY)
                                                .kIsGreaterThan(0.kFixed()),
                                            statement:
                                            Infinite.CurrentPlayer
                                                .kRead()
                                                .kGetRovi(uPlayerData.HAND)
                                                .kIOSelectOne()
                                                .kAbstractResolve()
                                                .kGetRovi(uResolved.INSTRUCTIONS)),

                                            // MOVE ACTION:
                                            kAction<uMoveAction>(
                                            condition:
                                            Infinite.CurrentPlayer
                                                .kRead()
                                                .kGetRovi(uPlayerData.ENERGY)
                                                .kIsGreaterThan(0.kFixed()),
                                            statement:
                                            Infinite.Template.NumericalMove
                                                .kWithRovi(uNumericalMove.DISTANCE, 1.kFixed().kRangeTo(4.kFixed()))
                                                .kWithRovi(
                                                Core.Hint<uNumericalMove>(),
                                                uMove.SUBJECT_COLLECTOR, 
                                                Infinite.AllUnits.kWhere(
                                                iUnit => iUnit.kRef()
                                                    .kRead()
                                                    .kGetRovi(uUnitData.OWNER)
                                                    .kEquals(Infinite.CurrentPlayer))
                                                .kMetaBoxed<IMulti<IRoveggi<uUnitIdentifier>>>([])))
                                        });
                                }).Invoke()
                                .kAsVariable(out var iPlayableActions),

                            // player objects:
                            iPlayerDeclarations.kRef()
                                .kMap(
                                iDeclaredPlayer =>
                                    Core.kSubEnvironment<IRoveggi<uPlayerData>>(
                                    new()
                                    {
                                        Environment =
                                        [
                                            iDeclaredPlayer.kRef()
                                                .kGetRovi(uPlayerDeclaration.DECK)
                                                .kAsVariable(out var iDeck),
                                            iDeclaredPlayer.kRef()
                                                .kGetRovi(uPlayerDeclaration.HAND_SIZE)
                                                .kAsVariable(out var iHandSize),
                                        ],
                                        Value =
                                            Core.kCompose<uPlayerData>()
                                                .kWithRovi(uPlayerData.HAND_SIZE, iHandSize.kRef())
                                                .kWithRovi(
                                                uPlayerData.HAND,
                                                iDeck.kRef().kGetSlice(1.kFixed().kRangeTo(iHandSize.kRef())))
                                                .kWithRovi(
                                                uPlayerData.STACK,
                                                iDeck.kRef().kGetSlice(iHandSize.kRef().kAdd(1.kFixed()).kRangeTo(iDeck.kRef().kCount())))
                                                .kWithRovi(uPlayerData.AVAILABLE_ACTIONS, iPlayableActions.kRef())
                                                .kWithRovi(uPlayerData.PERSPECTIVE_ROTATION, iDeclaredPlayer.kRef().kGetRovi(uPlayerDeclaration.PERSPECTIVE_ROTATION))
                                                .kWithRovi(uPlayerData.CONTROL, 0.kFixed())
                                                .kWithRovi(uPlayerData.ENERGY, 0.kFixed()),
                                    }))
                                .kAsVariable(out var iPlayerObjects),

                            // MAKE clear:
                            Core.kMulti<Rog>(
                                [
                                    Core.kAllRovedanggiKeys<uPlayerIdentifier, IRoveggi<uPlayerData>>()
                                        .kMap(
                                        iIdentifier =>
                                            iIdentifier.kRef().kRedact()),
                                    Core.kAllRovedanggiKeys<uHexIdentifier, IRoveggi<uHexData>>()
                                        .kMap(
                                        iIdentifier =>
                                            iIdentifier.kRef().kRedact()),
                                    Core.kAllRovedanggiKeys<uUnitIdentifier, IRoveggi<uUnitData>>()
                                        .kMap(
                                        iIdentifier =>
                                            iIdentifier.kRef().kRedact()),
                                ])
                                .kMetaBoxed([])
                                .kAsVariable(out var iMakeClear),

                            // MAKE game:
                            Infinite.Game.kWrite(
                                Core.kCompose<uGame>()
                                    .kWithRovi(uGame.ROTATION_COUNT, 0.kFixed())
                                    .kWithRovi(uGame.TURN_INDEX, 1.kFixed())
                                    .kWithRovi(uGame.TURN_ORDER, iPlayerIdentifiers.kRef())
                                    .kWithRovi(uGame.PLAYABLE_ACTIONS, iPlayableActions.kRef()))
                                .kMetaBoxed(
                                [
                                    iPlayerIdentifiers,
                                    iPlayableActions,
                                ])
                                .kAsVariable(out var iMakeGame),

                            // MAKE players:
                            iPlayerIdentifiers.kRef()
                                .kMap(
                                iPlayerIdentifier =>
                                    iPlayerIdentifier.kRef()
                                        .kWrite(iPlayerObjects.kRef().kGetIndex(iPlayerIdentifier.kRef().kGetRovi(uPlayerIdentifier.NUMBER))))
                                .kMetaBoxed(
                                [
                                    iPlayerIdentifiers,
                                    iPlayerObjects,
                                ])
                                .kAsVariable(out var iMakePlayers),

                            // MAKE map:
                            iConfig.kRef()
                                .kGetVarovaKeys(uGameConfiguration.MAP)
                                .kMap(
                                iCoordinate =>
                                    iCoordinate.kRef()
                                        .kWrite(
                                        Core.kCompose<uHexData>()
                                            .kWithRovi(uHexData.TYPE, iConfig.kRef().kGetVarovi(uGameConfiguration.MAP, iCoordinate.kRef()))))
                                .kMetaBoxed([])
                                .kAsVariable(out var iMakeMap),

                            // MAKE game modifiers:
                            iConfig.kRef()
                                .kGetRovi(uGameConfiguration.MODIFIERS)
                                .kMap(iModifier => iModifier.kRef().kExecute())
                                .kMetaBoxed([])
                                .kAsVariable(out var iMakeGameModifiers),

                            // MAKE player modifiers:
                            Infinite.Game.kRead()
                                .kGetRovi(uGame.TURN_ORDER)
                                .kMap(
                                iIdentifier =>
                                    iConfig.kRef()
                                        .kGetRovi(uGameConfiguration.PLAYERS)
                                        .kGetIndex(iIdentifier.kRef().kGetRovi(uPlayerIdentifier.NUMBER))
                                        .kGetRovi(uPlayerDeclaration.MODIFIERS)
                                        .kMap(
                                        iModifier =>
                                            iModifier.kRef()
                                                .kExecuteWith(
                                                new()
                                                {
                                                    A = iIdentifier.kRef(),
                                                })))
                                .kMetaBoxed([])
                                .kAsVariable(out var iMakePlayerModifiers),
                        ],
                        Value =
                            Core.kMulti<Rog>(
                            new()
                            {
                                iMakeClear.kRef().kExecute(),
                                iMakeGame.kRef().kExecute(),
                                iMakePlayers.kRef().kExecute(),
                                iMakeMap.kRef().kExecute(),
                                iMakeGameModifiers.kRef().kExecute(),
                                iMakePlayerModifiers.kRef().kExecute()
                            }),
                    }),
        };
}