namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Resolve;

using SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Move;
using SixShaded.FourZeroOne.Axois.Infinite.Rovetus.Constructs.Resolved;
using ResolvedObj = IRoveggi<u.Constructs.Resolved.uResolvedTurn>;
using Actions = IMulti<IRoveggi<u.Constructs.Resolved.uResolvedAction>>;
using Core = Core.Syntax.Core;
using u.Constructs;
using Infinite = Syntax.Infinite;
using u.Data;

public static class ResolveTurn
{
    private static IKorssa<Actions> DoTurn() =>
        Core.kMetaFunctionRecursive<Actions>(
            [],
            (iContinuedTurn) =>
                Core.kSubEnvironment<Actions>(
                new()
                {
                    Environment =
                    [
                        Infinite.CurrentPlayer
                            .kRead()
                            .kGetRovi(uPlayerData.AVAILABLE_ACTIONS)
                            .kWhere(
                            iAction =>
                                iAction.kRef()
                                    .kGetRovi(uPlayableAction.CONDITION)
                                    .kExecute())
                            .kAsVariable(out var iValidActions)
                    ],
                    Value =
                        iValidActions.kRef()
                            .kIOSelectOneCancellable(
                            Core.Hint<Actions>(),
                            new()
                            {
                                Select =
                                    iSelectedAction =>
                                        Core.kSubEnvironment<Actions>(
                                        new()
                                        {
                                            Environment =
                                            [
                                                iSelectedAction.kRef()
                                                    .kResolve()
                                                    .kAsVariable(out var iResolvedAction),
                                                iResolvedAction.kRef()
                                                    .kGetRovi(uResolved.INSTRUCTIONS)
                                            ],
                                            Value =
                                                iResolvedAction.kRef()
                                                    .kYield()
                                                    .kConcat(
                                                    iContinuedTurn.kRef()
                                                        .kExecute())
                                        }),
                                Cancel = () => Core.kMulti<IRoveggi<uResolvedAction>>([])
                            })
                }))
            .kExecute();

    public static Korvessa<IRoveggi<uTurn>, ResolvedObj> Construct(IKorssa<IRoveggi<uTurn>> turn) =>
        new(turn)
        {
            Du = Axoi.Korvedu("ResolveTurn"),
            Definition =
                (_, iTurn) =>
                    Core.kSubEnvironment<ResolvedObj>(
                    new()
                    {
                        Environment =
                        [
                            Infinite.Game
                                .kSafeUpdate(
                                iGame =>
                                    iGame.kRef()
                                        .kWithRovi(
                                        u.uGame.CURRENT_PLAYER,
                                        iTurn.kRef()
                                            .kGetRovi(uTurn.PLAYER))),
                            iTurn.kRef()
                                .kGetRovi(uTurn.PRE_TURN)
                                .kExecute()
                                .kAsVariable(out var iPreInstructions),
                            Core.kSubEnvironment<Actions>(
                                new()
                                {
                                    Environment = [iPreInstructions.kRef()],
                                    Value = DoTurn()
                                })
                                .kAsVariable(out var iActions),
                            iActions.kRef()
                                .kMap(
                                iAction =>
                                    iAction.kRef()
                                        .kGetRovi(uResolved.INSTRUCTIONS))
                                .kAsVariable(out var iTurnInstructions),
                            Core.kSubEnvironment<Rog>(
                                new()
                                {
                                    Environment =
                                    [
                                        iPreInstructions.kRef(),
                                        iTurnInstructions.kRef()
                                    ],
                                    Value =
                                        iTurn.kRef()
                                            .kGetRovi(uTurn.POST_TURN)
                                            .kExecute()
                                })
                                .kAsVariable(out var iPostInstructions)
                        ],
                        Value =
                            Core.kCompose<uResolvedTurn>()
                                .kWithRovi(uResolvedTurn.TURN, iTurn.kRef())
                                .kWithRovi(uResolvedTurn.ACTIONS, iActions.kRef())
                                .kWithRovi(
                                Core.Hint<uResolvedTurn>(),
                                uResolved.INSTRUCTIONS,
                                Core.kMulti<Rog>(
                                new()
                                {
                                    iPreInstructions.kRef(),
                                    iTurnInstructions.kRef(),
                                    iPostInstructions.kRef(),
                                }))
                    })


        };
}