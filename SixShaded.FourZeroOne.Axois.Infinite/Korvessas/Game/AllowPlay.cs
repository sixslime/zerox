namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Game;

using Rovetus.Constructs.Resolved;
using Actions = IMulti<IRoveggi<u.Constructs.Resolved.uResolvedAction>>;
using Core = Core.Syntax.Core;
using u.Constructs;
using Infinite = Syntax.Infinite;
using u.Data;
using u.Identifier;

public static class AllowPlay
{
    public static Korvessa<IRoveggi<uPlayerIdentifier>, Actions> Construct(IKorssa<IRoveggi<uPlayerIdentifier>> player) =>
        new(player)
        {
            Du = Axoi.Korvedu("AllowPlay"),
            Definition =
                (_, iPlayer) =>
                    Core.kSubEnvironment<Actions>(
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
                                        iPlayer.kRef())),
                        ],
                        Value =
                            Core.kMetaFunctionRecursive<Actions>(
                                [],
                                iContinuedTurn =>
                                    Core.kSubEnvironment<Actions>(
                                    new()
                                    {
                                        Environment =
                                        [
                                            Infinite.CurrentPlayer
                                                .kRead()
                                                .kGetRovi(uPlayerData.PLAYABLE_ACTIONS)
                                                .kWhere(
                                                iAction =>
                                                    iAction.kRef()
                                                        .kGetRovi(uPlayableAction.CONDITION)
                                                        .kExecute())
                                                .kAsVariable(out var iValidActions),
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
                                                                        .kGetRovi(uResolved.INSTRUCTIONS),
                                                                ],
                                                                Value =
                                                                    iResolvedAction.kRef()
                                                                        .kYield()
                                                                        .kConcat(
                                                                        iContinuedTurn.kRef()
                                                                            .kExecute()),
                                                            }),
                                                    Cancel = () => Core.kMulti<IRoveggi<uResolvedAction>>([]),
                                                }),
                                    }))
                                .kExecute(),
                    }),
        };
}