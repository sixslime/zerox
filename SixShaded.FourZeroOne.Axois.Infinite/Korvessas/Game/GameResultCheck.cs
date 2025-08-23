namespace SixShaded.FourZeroOne.Axois.Infinite.Korvessas.Game;

using u.Constructs.Move;
using u.Identifier;
using u.Data;
using u.Constructs.Resolved;
using u.Constructs;
using u;
using HexIdent = IRoveggi<u.Identifier.uHexIdentifier>;
using PlayerIdent = IRoveggi<u.Identifier.uPlayerIdentifier>;
using UnitIdent = IRoveggi<u.Identifier.uUnitIdentifier>;
using Core = Core.Syntax.Core;
using Infinite = Syntax.Infinite;
using u.Constructs.GameResults;

public static class GameResultCheck
{
    public static Korvessa<IRoveggi<uGameResult>> Construct() =>
        new()
        {
            Du = Axoi.Korvedu("GameResultCheck"),
            Definition =
                _ =>
                    Core.kSelector<IRoveggi<uGameResult>>(
                        new()
                        {
                            () =>
                                Core.kSubEnvironment<IRoveggi<uGameResult>>(
                                new()
                                {
                                    Environment =
                                    [
                                        Infinite.AllPlayers
                                            .kAccumulateInto(
                                            Core.kCompose<uWinConditionMet>(),
                                            (iObj, iPlayer) =>
                                                Core.kSubEnvironment<IRoveggi<uWinConditionMet>>(
                                                new()
                                                {
                                                    Environment =
                                                    [
                                                        iPlayer.kRef()
                                                            .kWinChecks()
                                                            .kAsVariable(out var iWinChecks),
                                                    ],
                                                    Value =
                                                        iWinChecks.kRef()
                                                            .kGetRovi(uCheckable.PASSED)
                                                            .kIfTrue<IRoveggi<uWinConditionMet>>(
                                                            new()
                                                            {
                                                                Then =
                                                                    iObj.kRef()
                                                                        .kWithVarovi(uWinConditionMet.PLAYERS, iPlayer.kRef(), iWinChecks.kRef()),
                                                                Else =
                                                                    iObj.kRef(),
                                                            }),
                                                }))
                                            .kAsVariable(out var iWinType),
                                    ],
                                    Value =
                                        iWinType.kRef()
                                            .kGetVarovaKeys(uWinConditionMet.PLAYERS)
                                            .kCount()
                                            .kIsGreaterThan(0.kFixed())
                                            .kIfTrue<IRoveggi<uGameResult>>(
                                            new()
                                            {
                                                Then = iWinType.kRef(),
                                                Else = Core.kNollaFor<IRoveggi<uGameResult>>(),
                                            }),
                                }),
                            () =>
                                Infinite.Game
                                    .kRead()
                                    .kGetRovi(uGame.DEAD_ROTATIONS)
                                    .kIsGreaterThan(10.kFixed())
                                    .kIfTrue<IRoveggi<uGameResult>>(
                                    new()
                                    {
                                        Then =
                                            Core.kCompose<uNoProgression>(),
                                        Else =
                                            Core.kNollaFor<IRoveggi<uGameResult>>(),
                                    }),
                            () =>
                                Infinite.Game
                                    .kRead()
                                    .kGetRovi(uGame.ROTATION_COUNT)
                                    .kIsGreaterThan(80.kFixed())
                                    .kIfTrue<IRoveggi<uGameResult>>(
                                    new()
                                    {
                                        Then =
                                            Core.kCompose<uTooLong>(),
                                        Else =
                                            Core.kNollaFor<IRoveggi<uGameResult>>(),
                                    }),
                        })
                        .kWithRovi(uGameResult.FINAL_STATE, Core.kGetProgramState()),
        };
}