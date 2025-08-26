namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Game;

using Rovetus;
using Rovetus.Config;
using Rovetus.Constructs;
using Rovetus.Constructs.ActionTypes;
using Rovetus.Constructs.HexTypes;
using Rovetus.Constructs.Move;
using Rovetus.Constructs.Resolved;
using Rovetus.Data;
using Rovetus.Identifier;
using Core = Core.Syntax.Core;
using Infinite = Syntax.Infinite;

public record SetupGame(IKorssa<IRoveggi<uGameConfiguration>> config) : Korvessa<IRoveggi<uGameConfiguration>, Rog>(config)
{
    protected override RecursiveMetaDefinition<IRoveggi<uGameConfiguration>, Rog> InternalDefinition() =>
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
                    CreatePlayableActions()
                        .kAsVariable(out var iPlayableActions),

                    // map:
                    iConfig.kRef()
                        .kGetRovi(uGameConfiguration.MAP)
                        .kAsVariable(out var iMap),

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
                                        .kWithRovi(uPlayerData.PLAYABLE_ACTIONS, iPlayableActions.kRef())
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
                            .kWithRovi(uGame.DEAD_ROTATIONS, 0.kFixed())
                            .kWithRovi(uGame.TURN_INDEX, 1.kFixed())
                            .kWithRovi(uGame.TURN_ORDER, iPlayerIdentifiers.kRef())
                            .kWithRovi(uGame.LAST_ROTATION_STATE, Core.kGetProgramState()))
                        .kMetaBoxed([])
                        .kAsVariable(out var iMakeGame),

                    // MAKE players:
                    iPlayerIdentifiers.kRef()
                        .kMap(
                        iPlayerIdentifier =>
                            iPlayerIdentifier.kRef()
                                .kWrite(iPlayerObjects.kRef().kGetIndex(iPlayerIdentifier.kRef().kGetRovi(uPlayerIdentifier.NUMBER))))
                        .kMetaBoxed([])
                        .kAsVariable(out var iMakePlayers),

                    // MAKE map:
                    iMap.kRef()
                        .kGetVarovaKeys(uMapDeclaration.HEXES)
                        .kMap(
                        iPos =>
                            iPos.kRef()
                                .kWrite(
                                Core.kCompose<uHexData>()
                                    .kWithRovi(uHexData.TYPE, iMap.kRef().kGetVarovi(uMapDeclaration.HEXES, iPos.kRef()))))
                        .kMetaBoxed([])
                        .kAsVariable(out var iMakeMap),

                    // MAKE units:
                    iMap.kRef()
                        .kGetVarovaKeys(uMapDeclaration.UNIT_SPAWNS)
                        .kMapWithIndex(
                        (iSpawnPos, iIndex) =>
                            Core.kCompose<uUnitIdentifier>()
                                .kWithRovi(uUnitIdentifier.NUMBER, iIndex.kRef())
                                .kWrite(
                                Core.kCompose<uUnitData>()
                                    .kWithRovi(
                                    uUnitData.OWNER,
                                    iPlayerIdentifiers.kRef()
                                        .kGetIndex(
                                        iMap.kRef()
                                            .kGetVarovi(uMapDeclaration.UNIT_SPAWNS, iSpawnPos.kRef())))

                                    .kWithRovi(uUnitData.POSITION, iSpawnPos.kRef())
                                    .kWithRovi(uUnitData.EFFECTS, Core.kMulti<IRoveggi<uUnitEffect>>([]))))
                        .kMetaBoxed([])
                        .kAsVariable(out var iMakeUnits),

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
                        iMakeUnits.kRef().kExecute(),
                        iMakeGameModifiers.kRef().kExecute(),
                        iMakePlayerModifiers.kRef().kExecute(),
                    }),
            });

    private static IKorssa<IRoveggi<uPlayableAction>> CreateAction<C>(IKorssa<Bool> condition, IKorssa<Rog> statement)
        where C : uActionType, IConcreteRovetu =>
        Core.kCompose<uPlayableAction>()
            .kWithRovi(uPlayableAction.CONDITION, condition.kMetaBoxed([]))
            .kWithRovi(uPlayableAction.STATEMENT, statement.kMetaBoxed([]))
            .kWithRovi(uPlayableAction.TYPE, Core.kCompose<C>().kIsType<IRoveggi<uActionType>>());

    private static IKorssa<IMulti<IRoveggi<uPlayableAction>>> CreatePlayableActions() =>
        Core.kMulti<IRoveggi<uPlayableAction>>(
        new()
        {
            // ABILITY ACTION:
            CreateAction<uAbilityAction>(
            Infinite.CurrentPlayer
                .kRead()
                .kGetRovi(uPlayerData.ENERGY)
                .kIsGreaterThan(0.kFixed())
                .kAnd(
                Infinite.CurrentPlayer
                    .kRead()
                    .kGetRovi(uPlayerData.HAND)
                    .kCount()
                    .kIsGreaterThan(0.kFixed())),
            Core.kSubEnvironment<Rog>(
            new()
            {
                Environment =
                [
                    Infinite.CurrentPlayer
                        .kRead()
                        .kGetRovi(uPlayerData.HAND)
                        .kIOSelectOne()
                        .kAsVariable(out var iSelectedAbility),
                ],
                Value =
                    Core.kMulti<Rog>(
                    new()
                    {
                        Infinite.CurrentPlayer
                            .kUpdate(
                            iPlayerData =>
                                iPlayerData.kRef()
                                    .kSafeUpdateRovi(
                                    uPlayerData.HAND,
                                    iHand =>
                                        iHand.kRef().kExcept(iSelectedAbility.kRef().kYield()))
                                    .kSafeUpdateRovi(
                                    uPlayerData.STACK,
                                    iStack =>
                                        iStack.kRef()
                                            .kConcat(iSelectedAbility.kRef().kYield()))),
                        iSelectedAbility.kRef()
                            .kAbstractResolve()
                            .kGetRovi(uResolved.INSTRUCTIONS),
                    }),
            })),

            // MOVE ACTION:
            CreateAction<uMoveAction>(
            Infinite.CurrentPlayer
                .kRead()
                .kGetRovi(uPlayerData.ENERGY)
                .kIsGreaterThan(0.kFixed()),
            Infinite.Template.NumericalMove
                .kWithRovi(uNumericalMove.DISTANCE, 1.kFixed().kRangeTo(4.kFixed()))
                .kWithRovi(
                Core.Hint<uNumericalMove>(),
                uMove.SUBJECT_COLLECTOR,
                Infinite.AllUnits.kWhere(
                    iUnit =>
                        iUnit.kRef().kRead().kGetRovi(uUnitData.OWNER).kEquals(Infinite.CurrentPlayer))
                    .kMetaBoxed<IMulti<IRoveggi<uUnitIdentifier>>>([]))
                .kResolve()
                .kMap(iResolvedMove => iResolvedMove.kRef().kGetRovi(uResolved.INSTRUCTIONS))),

            // DISCARD ACTION:
            CreateAction<uDiscardAction>(
            Infinite.CurrentPlayer
                .kRead()
                .kGetRovi(uPlayerData.ENERGY)
                .kIsGreaterThan(0.kFixed())
                .kAnd(
                Infinite.CurrentPlayer
                    .kRead()
                    .kGetRovi(uPlayerData.HAND)
                    .kCount()
                    .kIsGreaterThan(0.kFixed())),
            Core.kSubEnvironment<Rog>(
            new()
            {
                Environment =
                [
                    Infinite.CurrentPlayer
                        .kRead()
                        .kGetRovi(uPlayerData.HAND)
                        .kIOSelectMultiple(1.kFixed().kRangeTo(2.kFixed()))
                        .kAsVariable(out var iSelectedDiscards),
                ],
                Value =
                    Infinite.CurrentPlayer
                        .kUpdate(
                        iPlayerData =>
                            iPlayerData.kRef()
                                .kSafeUpdateRovi(
                                uPlayerData.HAND,
                                iHand =>
                                    iHand.kRef()
                                        .kExcept(iSelectedDiscards.kRef()))
                                .kSafeUpdateRovi(
                                uPlayerData.STACK,
                                iStack =>
                                    iStack.kRef()
                                        .kConcat(iSelectedDiscards.kRef()))),
            })),

            // DOMINATE ACTION:
            CreateAction<uDominateAction>(
            Infinite.CurrentPlayer
                .kRead()
                .kGetRovi(uPlayerData.ENERGY)
                .kIsGreaterThan(1.kFixed())
                .ksLazyAnd(
                Core.kSubEnvironment<Bool>(
                new()
                {
                    Environment =
                    [
                        Core.kMetaFunction<IRoveggi<uUnitIdentifier>, Bool>(
                            [],
                            iUnit =>
                                iUnit.kRef()
                                    .kRead()
                                    .kGetRovi(uUnitData.POSITION)
                                    .kRead()
                                    .kGetRovi(uHexData.TYPE)
                                    .kIsType<uControlHex>()
                                    .kExists())
                            .kAsVariable(out var iOnControlHex),
                    ],
                    Value =
                        Infinite.AllUnits
                            .kAnyMatch(
                            iUnit =>
                                iUnit.kRef()
                                    .kRead()
                                    .kGetRovi(uUnitData.OWNER)
                                    .kEquals(Infinite.CurrentPlayer)
                                    .ksLazyAnd(
                                    iOnControlHex.kRef()
                                        .kExecuteWith(
                                        new()
                                        {
                                            A = iUnit.kRef(),
                                        })))
                            .ksLazyAnd(
                            Infinite.AllUnits
                                .kAnyMatch(
                                iUnit =>
                                    iUnit.kRef()
                                        .kRead()
                                        .kGetRovi(uUnitData.OWNER)
                                        .kEquals(Infinite.CurrentPlayer)
                                        .kNot()
                                        .ksLazyAnd(
                                        iOnControlHex.kRef()
                                            .kExecuteWith(
                                            new()
                                            {
                                                A = iUnit.kRef(),
                                            })))
                                .kNot()),
                })),
            Infinite.CurrentPlayer
                .kUpdate(
                iPlayerData =>
                    iPlayerData.kRef()
                        .kUpdateRovi(
                        uPlayerData.CONTROL,
                        iControl =>
                            iControl.kRef().kAdd(1.kFixed())))),
        });
}
